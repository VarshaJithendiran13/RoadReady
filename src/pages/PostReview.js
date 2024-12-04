import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import styled from "styled-components";

// Styled Components
const Container = styled.div`
  background-color: #121212;
  color: #e0e0e0;
  padding: 20px;
  min-height: 100vh;
  font-family: "Roboto", sans-serif;
  display: flex;
  flex-direction: column;
  align-items: center;
`;

const Form = styled.form`
  width: 100%;
  max-width: 600px;
  background-color: #1e1e1e;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.2);
`;

const InputGroup = styled.div`
  margin-bottom: 15px;
  display: flex;
  flex-direction: column;
`;

const Label = styled.label`
  font-size: 1.1em;
  margin-bottom: 5px;
`;

const Input = styled.input`
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 1em;
  background-color: #2e2e2e;
  color: white;
`;

const TextArea = styled.textarea`
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 1em;
  background-color: #2e2e2e;
  color: white;
  resize: none;
`;

const Button = styled.button`
  padding: 10px 20px;
  background-color: #0d47a1;
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 1.1em;
  cursor: pointer;

  &:hover {
    background-color: #1565c0;
  }
`;

const PostReview = () => {
  const navigate = useNavigate();
  const { state } = useLocation();
  const { carId } = state;

  const [rating, setRating] = useState(5);
  const [comment, setComment] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    const reviewDate = new Date().toISOString();

    // Prepare review data (no need for userId)
    const reviewData = {
      carId: carId,
      rating: rating,
      comment: comment,
      reviewDate: reviewDate,
    };

    try {
      const response = await fetch("https://localhost:7173/api/Review", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
        body: JSON.stringify(reviewData),
      });

      if (!response.ok) {
        throw new Error("Failed to post review.");
      }

      alert("Review posted successfully!");
      navigate(`/reservenow/${carId}`); // Redirect back to the ReserveNow page
    } catch (error) {
      console.error(error.message);
      alert("An error occurred while posting the review.");
    }
  };

  return (
    <Container>
      <h2>Write a Review</h2>
      <Form onSubmit={handleSubmit}>
        <InputGroup>
          <Label htmlFor="rating">Rating (1-5)</Label>
          <Input
            type="number"
            id="rating"
            value={rating}
            onChange={(e) => setRating(Number(e.target.value))}
            min="1"
            max="5"
            required
          />
        </InputGroup>
        <InputGroup>
          <Label htmlFor="comment">Comment</Label>
          <TextArea
            id="comment"
            rows="5"
            value={comment}
            onChange={(e) => setComment(e.target.value)}
            required
          />
        </InputGroup>
        <Button type="submit">Submit Review</Button>
      </Form>
    </Container>
  );
};

export default PostReview;
