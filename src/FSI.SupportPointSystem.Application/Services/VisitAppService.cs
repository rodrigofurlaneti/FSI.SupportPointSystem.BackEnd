using FSI.SupportPointSystem.Application.Interfaces;
using FSI.SupportPointSystem.Domain.Interfaces.Repositories;
using FSI.SupportPointSystem.Domain.Interfaces.Services;
using FSI.SupportPointSystem.Domain.Entities;
using FSI.SupportPointSystem.Domain.ValueObjects;
using FSI.SupportPointSystem.Domain.Exceptions;
using FluentValidation;
using FSI.SupportPointSystem.Application.Dtos.Checkin.Request;
using FSI.SupportPointSystem.Application.Dtos.Checkout.Request;
using FSI.SupportPointSystem.Application.Dtos.Visit.Response;

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
            var validationResult = await _checkinValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new BusinessRuleException(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage)));
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
                ?? throw new DomainException("Cliente não encontrado.");
            var activeVisit = await _visitRepository.GetActiveVisitBySellerIdAsync(request.SellerId);
            if (activeVisit != null)
                throw new BusinessRuleException("O vendedor já possui um check-in ativo.");
            var sellerLoc = new Coordinates(request.Latitude, request.Longitude);
            double distance = _locationService.CalculateDistanceInMeters(sellerLoc, customer.LocationTarget);
            var visit = new Visit(request.SellerId, request.CustomerId, sellerLoc, distance);
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
            var validationResult = await _checkoutValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new BusinessRuleException(string.Join(" ", validationResult.Errors.Select(e => e.ErrorMessage)));
            var visit = await _visitRepository.GetActiveVisitBySellerIdAsync(request.SellerId)
                ?? throw new BusinessRuleException("Não há check-in ativo para este vendedor.");
            var customer = await _customerRepository.GetByIdAsync(visit.CustomerId)
                ?? throw new DomainException("Dados do cliente da visita não encontrados.");
            var currentLoc = new Coordinates(request.Latitude, request.Longitude);
            double distance = _locationService.CalculateDistanceInMeters(currentLoc, customer.LocationTarget);
            visit.PerformCheckout(currentLoc, distance, request.SummaryCheckOut);
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