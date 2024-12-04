import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import styled from "styled-components";

const PaymentContainer = styled.div`
  background-color: #121212;
  color: #e0e0e0;
  padding: 20px;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const PaymentDetails = styled.div`
  background-color: #1e1e1e;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.2);
  max-width: 600px;
  text-align: center;
  margin-bottom: 20px;
`;

const InputGroup = styled.div`
  margin: 10px 0;
`;

const Label = styled.label`
  display: block;
  margin-bottom: 5px;
`;

const Input = styled.input`
  width: 100%;
  padding: 10px;
  border-radius: 5px;
  border: 1px solid #ddd;
`;

const Button = styled.button`
  padding: 10px 20px;
  background-color: #0d47a1;
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 1.1em;
  cursor: pointer;
  margin-top: 20px;

  &:hover {
    background-color: #1565c0;
  }
`;

const Payment = () => {
  const location = useLocation();
  const navigate = useNavigate();

  // Extract payment details from location state
  // Check if location.state is coming through
  console.log("Payment Page - Location State:", location.state);
  const { totalAmount, carName, pickupDate, dropOffDate, reservationId } = location.state || {};

  // Form states for the payment form
  const [paymentMethod, setPaymentMethod] = useState("Credit Card");
  const [cardNumber, setCardNumber] = useState("");
  const [expiryDate, setExpiryDate] = useState("");
  const [cvv, setCvv] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  // Handle payment submission
  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!cardNumber || !expiryDate || !cvv) {
      setErrorMessage("Please fill in all payment details.");
      return;
    }

    // Simulate a successful payment
    const paymentDetails = {
      reservationId: reservationId,
      amount: totalAmount,
      paymentDate: new Date().toISOString(),
      paymentMethod: paymentMethod,
      status: "Completed", // Simulating a successful payment
    };

    try {
      const response = await fetch("https://localhost:7173/api/Payment", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify(paymentDetails),
      });

      if (!response.ok) {
        throw new Error(`Payment API failed with status ${response.status}`);
      }

      alert("Payment successful and recorded!");
      navigate("/roadreadyhome"); // Redirect to home page
    } catch (error) {
      console.error("Error posting payment details:", error);
      alert("Payment recorded, but failed to update the database.");
    }
  };

  // Check if the payment details are invalid
  if (!totalAmount || !reservationId) {
    return (
      <PaymentContainer>
        <h2>Invalid Payment Details</h2>
        <Button onClick={() => navigate("/")}>Go Back</Button>
      </PaymentContainer>
    );
  }

  return (
    <PaymentContainer>
      <h2>Payment Details</h2>
      <PaymentDetails>
        <p><strong>CarName:</strong> {carName} </p>
        <p><strong>Pickup Date:</strong> {pickupDate}</p>
        <p><strong>Drop-off Date:</strong> {dropOffDate}</p>
        <p><strong>Total Amount:</strong> ₹{totalAmount}</p>
      </PaymentDetails>

      {/* Payment Form */}
      {errorMessage && <p style={{ color: "red" }}>{errorMessage}</p>}

      <form onSubmit={handleSubmit}>
        <InputGroup>
          <Label>Payment Method</Label>
          <select
            value={paymentMethod}
            onChange={(e) => setPaymentMethod(e.target.value)}
            required
          >
            <option value="Credit Card">Credit Card</option>
            <option value="Debit Card">Debit Card</option>
            <option value="PayPal">PayPal</option>
            <option value="Net Banking">Net Banking</option>
          </select>
        </InputGroup>

        {paymentMethod !== "PayPal" && (
          <>
            <InputGroup>
              <Label>Card Number</Label>
              <Input
                type="text"
                value={cardNumber}
                onChange={(e) => setCardNumber(e.target.value)}
                placeholder="Enter your card number"
                required
              />
            </InputGroup>

            <InputGroup>
              <Label>Expiry Date</Label>
              <Input
                type="text"
                value={expiryDate}
                onChange={(e) => setExpiryDate(e.target.value)}
                placeholder="MM/YY"
                required
              />
            </InputGroup>

            <InputGroup>
              <Label>CVV</Label>
              <Input
                type="text"
                value={cvv}
                onChange={(e) => setCvv(e.target.value)}
                placeholder="Enter your CVV"
                required
              />
            </InputGroup>
          </>
        )}

        <Button type="submit">Pay ₹{totalAmount}</Button>
      </form>
    </PaymentContainer>
  );
};

export default Payment;
