using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Application.Dtos;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.Interfaces.Services;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.ValueObjects;
using FSI.SupportPointSystem.Domain.Exceptions;
using FluentValidation;

namespace FSI.SupportPointSystem.Application.Services
{
    public class VisitAppService : IVisitAppService
    {
        private readonly IVisitRepository _visitRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILocationService _locationService;
        private readonly IValidator<CheckinRequest> _checkinValidator;
        private readonly IValidator<CheckoutRequest> _checkoutValidator;

        public VisitAppService(
            IVisitRepository visitRepository,
            ICustomerRepository customerRepository,
            ILocationService locationService,
            IValidator<CheckinRequest> checkinValidator,
            IValidator<CheckoutRequest> checkoutValidator)
        {
            _visitRepository = visitRepository;
            _customerRepository = customerRepository;
            _locationService = locationService;
            _checkinValidator = checkinValidator;
            _checkoutValidator = checkoutValidator;
        }

        public async Task<VisitResponse> RegisterCheckinAsync(CheckinRequest request)
        {
            // 1. Validação da Requisição (Application Level)
            var validationResult = await _checkinValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new BusinessRuleException(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage)));

            // 2. Regra de Negócio: Cliente existe?
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
                ?? throw new DomainException("Cliente não encontrado.");

            // 3. Regra de Negócio: Vendedor já está em uma visita?
            var activeVisit = await _visitRepository.GetActiveVisitBySellerIdAsync(request.SellerId);
            if (activeVisit != null)
                throw new BusinessRuleException("O vendedor já possui um check-in ativo.");

            // 4. Cálculo de Distância e Criação da Entidade
            var sellerLoc = new Coordinates(request.Latitude, request.Longitude);
            double distance = _locationService.CalculateDistanceInMeters(sellerLoc, customer.LocationTarget);

            // O construtor de Visit validará se distance <= 100m
            var visit = new Visit(request.SellerId, request.CustomerId, sellerLoc, distance);

            // 5. Persistência
            await _visitRepository.SaveCheckinAsync(visit);

            return new VisitResponse
            {
                VisitId = visit.Id,
                Message = "Check-in realizado com sucesso.",
                DistanceMeters = distance,
                Timestamp = visit.CheckinTimestamp
            };
        }

        public async Task<VisitResponse> RegisterCheckoutAsync(CheckoutRequest request)
        {
            // 1. Validação da Requisição
            var validationResult = await _checkoutValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new BusinessRuleException(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage)));

            // 2. Regra de Negócio: Existe visita aberta?
            var visit = await _visitRepository.GetActiveVisitBySellerIdAsync(request.SellerId)
                ?? throw new BusinessRuleException("Não há check-in ativo para este vendedor.");

            // 3. Regra de Negócio: Validar cliente da visita
            var customer = await _customerRepository.GetByIdAsync(visit.CustomerId)
                ?? throw new DomainException("Dados do cliente da visita não encontrados.");

            // 4. Cálculo de Distância e Fechamento
            var currentLoc = new Coordinates(request.Latitude, request.Longitude);
            double distance = _locationService.CalculateDistanceInMeters(currentLoc, customer.LocationTarget);

            // O método PerformCheckout validará se distance <= 100m e calculará a duração
            visit.PerformCheckout(currentLoc, distance);

            // 5. Atualização
            await _visitRepository.SaveCheckoutAsync(visit);

            return new VisitResponse
            {
                VisitId = visit.Id,
                Message = $"Check-out realizado. Duração: {visit.DurationMinutes} min.",
                DistanceMeters = distance,
                Timestamp = visit.CheckoutTimestamp!.Value
            };
        }
    }
}