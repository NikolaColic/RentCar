using rent_a_car.DatabaseBroker;
using rent_a_car.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace rent_a_car.Server.System_operations
{
    internal class CreateReservation : GeneralSystemOperation
    {
        internal override object Execute(Domain.DomainObject domainObject)
        {
            Reservation reservation = (Reservation)domainObject;
            reservation.User.UserID = 1;
            reservation.ReservationID = Broker.Instance.GetID(reservation);
            reservation.User = (User)Broker.Instance.GetOneForID(reservation.User);

            Broker.Instance.Insert(reservation);

            return reservation;
        }
    }
}
