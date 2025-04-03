import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import styled, { keyframes } from "styled-components";
import { FaStar } from "react-icons/fa";

// Animations
const fadeIn = keyframes`
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
`;

const buttonHover = keyframes`
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.05);
  }
  100% {
    transform: scale(1);
  }
`;

// Styled Components
const Container = styled.div`
  background: linear-gradient(135deg, #0f2027, #203a43, #2c5364);
  color: #e0e0e0;
  padding: 20px;
  min-height: 100vh;
  font-family: "Roboto", sans-serif;
  display: flex;
  flex-direction: column;
  align-items: center;
  animation: ${fadeIn} 0.8s ease-in-out;
`;

const Heading = styled.h2`
  font-size: 2.5em;
  margin-bottom: 20px;
  text-align: center;
  color: #ffffff;
  text-shadow: 0 4px 6px rgba(0, 0, 0, 0.2);
`;

const Form = styled.form`
  width: 100%;
  max-width: 600px;
  background: rgba(30, 30, 30, 0.9);
  padding: 25px;
  border-radius: 15px;
  box-shadow: 0 10px 15px rgba(0, 0, 0, 0.5);
  animation: ${fadeIn} 1s ease-in-out;
`;

const InputGroup = styled.div`
  margin-bottom: 20px;
  display: flex;
  flex-direction: column;
`;

const Label = styled.label`
  font-size: 1.2em;
  margin-bottom: 8px;
  color: #cccccc;
`;

const TextArea = styled.textarea`
  padding: 12px;
  border: 1px solid #444;
  border-radius: 8px;
  font-size: 1.1em;
  background-color: #1e1e1e;
  color: white;
  resize: none;
  transition: all 0.3s ease;

  &:focus {
    border-color: #0d47a1;
    outline: none;
    background-color: #2e2e2e;
  }
`;

const Button = styled.button`
  padding: 12px 20px;
  background-color: #0d47a1;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1.2em;
  cursor: pointer;
  transition: all 0.3s ease;
  animation: ${fadeIn} 1.2s ease-in-out;

  &:hover {
    background-color: #1565c0;
    animation: ${buttonHover} 0.5s ease-in-out;
  }
`;

const Stars = styled.div`
  display: flex;
  justify-content: center;
  margin-bottom: 15px;

  .star {
    font-size: 2rem;
    color: #ccc;
    cursor: pointer;
    transition: color 0.2s ease;

    &:hover,
    &.selected {
      color: #ffd700;
    }
  }
`;

const PostReview = () => {
  const navigate = useNavigate();
  const { state } = useLocation();
  const { carId } = state;

  const [rating, setRating] = useState(5);
  const [comment, setComment] = useState("");

  const handleStarClick = (value) => {
    setRating(value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const reviewDate = new Date().toISOString();

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
      navigate(`/reservenow/${carId}`);
    } catch (error) {
      console.error(error.message);
      alert("An error occurred while posting the review.");
    }
  };

  return (
    <Container>
      <Heading>Write a Review</Heading>
      <Form onSubmit={handleSubmit}>
        <InputGroup>
          <Label>Rating</Label>
          <Stars>
            {[1, 2, 3, 4, 5].map((value) => (
              <FaStar
                key={value}
                className={`star ${value <= rating ? "selected" : ""}`}
                onClick={() => handleStarClick(value)}
              />
            ))}
          </Stars>
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
