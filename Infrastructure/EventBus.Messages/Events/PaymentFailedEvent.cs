namespace EventBus.Messages.Events
{
    public record PaymentFailedEvent : BaseIntegrationEvent
    {
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public string Reason { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
