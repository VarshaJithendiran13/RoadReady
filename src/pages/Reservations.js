import React, { useState, useEffect } from "react";
import { Box, Typography, Button, List, ListItem, ListItemText } from "@mui/material";

const Reservations = () => {
  const [reservations, setReservations] = useState([]);

  // Fetch user's reservations
  useEffect(() => {
    fetch("https://localhost:7173/api/Reservation/user/reservations", {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .then((res) => res.json())
      .then((data) => setReservations(data))
      .catch((err) => console.error("Error fetching reservations:", err));
  }, []);

  // Cancel reservation
  const handleCancelReservation = (reservationId) => {
    fetch(`https://localhost:7173/api/Reservation/${reservationId}`, {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .then((res) => {
        if (res.ok) {
          alert("Reservation canceled successfully!");
          setReservations(reservations.filter((r) => r.id !== reservationId));
        } else {
          alert("Failed to cancel reservation.");
        }
      })
      .catch((err) => console.error("Error canceling reservation:", err));
  };

  return (
    <Box sx={{ padding: 2 }}>
      <Typography variant="h4" gutterBottom>
        Your Reservations
      </Typography>

      {reservations.length > 0 ? (
        <List>
          {reservations.map((reservation) => (
            <ListItem key={reservation.id} sx={{ borderBottom: "1px solid #ddd", marginBottom: 2 }}>
              <ListItemText
                primary={`Car: ${reservation.carName}`}
                secondary={`Start Date: ${reservation.startDate} | End Date: ${reservation.endDate}`}
              />
              <Button
                onClick={() => handleCancelReservation(reservation.id)}
                color="error"
                variant="contained"
                sx={{
                  marginLeft: 2,
                }}
              >
                Cancel Reservation
              </Button>
            </ListItem>
          ))}
        </List>
      ) : (
        <Typography variant="h6" color="textSecondary">
          You have no reservations at the moment.
        </Typography>
      )}
    </Box>
  );
};

export default Reservations;
