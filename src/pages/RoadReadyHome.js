import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { FaCar, FaSearch, FaSortAmountDown, FaSortAmountUp, FaHome, FaUser, FaSignOutAlt } from "react-icons/fa";
import styled from "styled-components";

// Styled components for the styles
const Container = styled.div`
  background-color: #121212;
  color: #e0e0e0;
  min-height: 100vh;
  font-family: "Roboto", sans-serif;
`;

const Header = styled.header`
  background-color: #0d47a1;
  padding: 10px 20px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  color: white;
`;

const NavLink = styled(Link)`
  color: white;
  text-decoration: none;
  margin: 0 10px;
`;

const Button = styled.button`
  color: white;
  background: none;
  border: none;
  margin: 0 10px;
`;

const Filters = styled.div`
  display: flex;
  justify-content: center;
  margin: 20px;
`;

const Select = styled.select`
  margin: 10px;
`;

const CarList = styled.div`
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 20px;
`;

const CarCard = styled.div`
  width: 100%;
  max-width: 500px;
  height: auto;
  background-color: #1e1e1e;
  color: white;
  border-radius: 10px;
  display: flex;
  justify-content: space-between;
  padding: 20px;
  box-sizing: border-box;
`;

const CarImage = styled.img`
  width: 150px;
  height: 150px;
  object-fit: cover;
  border-radius: 8px;
`;

const CarDetails = styled.div`
  flex: 1;
  padding-left: 20px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
`;

const CarName = styled.h3`
  font-size: 1.5em;
  margin: 0;
`;

const CarPrice = styled.p`
  font-size: 1.2em;
  margin: 10px 0;
`;

const CarDescription = styled.p`
  flex-grow: 1;
`;

const CarLocation = styled.p`
  margin: 5px 0;
`;

const CarAvailability = styled.p`
  margin: 5px 0;
`;

const ReserveButton = styled(Link)`
  align-self: flex-start;
  padding: 10px 20px;
  background-color: #0d47a1;
  color: white;
  text-decoration: none;
  border-radius: 5px;
  margin-top: 10px;

  &:hover {
    background-color: #1565c0;
  }
`;

const Pagination = styled.div`
  text-align: center;
`;

const PaginationButton = styled.button`
  margin: 5px;
`;

const RoadReadyHome = () => {
  const [cars, setCars] = useState([]);
  const [cities] = useState([
    "Mumbai", "Delhi", "Bangalore", "Hyderabad", "Chennai", "Kolkata", "Pune", "Ahmedabad", "Jaipur",
  ]);
  const [filters, setFilters] = useState({
    location: "",
    priceRange: "",
    availability: "",
  });
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 6;
  const [sortedCars, setSortedCars] = useState([]);
  const [isPriceAscending, setIsPriceAscending] = useState(true);

  // Fetch cars from API
  const fetchCars = async () => {
    try {
      const response = await fetch("https://localhost:7173/api/Car");
      if (!response.ok) {
        throw new Error(`API responded with status ${response.status}`);
      }
      const data = await response.json();
      console.log("Fetched Cars:", data); // Debug the fetched data
      setCars(data);
      setSortedCars(data);
    } catch (error) {
      console.error("Error fetching car data:", error.message);
    }
  };

  useEffect(() => {
    fetchCars();
  }, []);

  // Filter cars based on user selection
  const handleSearch = () => {
    let filteredCars = cars;

    if (filters.location) {
      filteredCars = filteredCars.filter(
        (car) => car.location === filters.location
      );
    }

    if (filters.priceRange) {
      const [min, max] = filters.priceRange.split("-").map(Number);
      filteredCars = filteredCars.filter(
        (car) => car.pricePerDay >= min && car.pricePerDay <= max
      );
    }

    if (filters.availability !== "") {
      const availability = filters.availability === "Available";
      filteredCars = filteredCars.filter(
        (car) => car.availabilityStatus === availability
      );
    }

    setSortedCars(filteredCars);
    setCurrentPage(1);
  };

  // Sort cars by price
  const handleSortByPrice = () => {
    const sorted = [...sortedCars].sort((a, b) =>
      isPriceAscending ? a.pricePerDay - b.pricePerDay : b.pricePerDay - a.pricePerDay
    );
    setSortedCars(sorted);
    setIsPriceAscending(!isPriceAscending);
  };

  // Pagination
  const handlePageChange = (pageNumber) => {
    setCurrentPage(pageNumber);
  };

  const paginatedCars = sortedCars.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  return (
    <Container>
      {/* Header */}
      <Header>
        <div className="roadready-logo">
          <FaCar /> RoadReady
        </div>
        <nav>
          <NavLink to="/"> <FaHome /> Home </NavLink>
          <Button onClick={fetchCars}> <FaSearch /> Browse Cars </Button>
          <NavLink to="/reservations"> <FaCar /> Reservations </NavLink>
          <NavLink to="/profile"> <FaUser /> Profile </NavLink>
          <NavLink to="/logout"> <FaSignOutAlt /> Logout </NavLink>
        </nav>
      </Header>

      {/* Search Filters */}
      <Filters>
        <Select onChange={(e) => setFilters({ ...filters, location: e.target.value })}>
          <option value="">Select Location</option>
          {cities.map(city => <option key={city} value={city}>{city}</option>)}
        </Select>
        <Select onChange={(e) => setFilters({ ...filters, priceRange: e.target.value })}>
          <option value="">Select Price Range</option>
          <option value="0-1000">₹0-₹1000</option>
          <option value="1000-5000">₹1000-₹5000</option>
          <option value="5000-10000">₹5000-₹10000</option>
        </Select>
        <Select onChange={(e) => setFilters({ ...filters, availability: e.target.value })}>
          <option value="">Select Availability</option>
          <option value="Available">Available</option>
          <option value="Not Available">Not Available</option>
        </Select>
        <Button onClick={handleSearch}>Search</Button>
        <Button onClick={handleSortByPrice}>
          Sort by Price {isPriceAscending ? <FaSortAmountDown /> : <FaSortAmountUp />}
        </Button>
      </Filters>

      {/* Cars Listing */}
      <CarList>
  {paginatedCars.length > 0 ? (
    paginatedCars.map((car) => (
      <CarCard key={car.carId}>
        <CarImage src={car.imageUrl} alt={`${car.make} ${car.model}`} />
        <CarDetails>
          <CarName>{`${car.make} ${car.model}`}</CarName> {/* Combine make and model */}
          <CarPrice>₹{car.pricePerDay}/day</CarPrice>
          <CarDescription>{car.specifications}</CarDescription> {/* Use specifications */}
          <CarLocation>Location: {car.location}</CarLocation>
          <CarAvailability>Status: {car.availabilityStatus ? "Available" : "Not Available"}</CarAvailability> {/* Render availability */}
          <ReserveButton to={`/reservenow/${car.carId}`}>Reserve Now</ReserveButton> {/* Use carId for reservation link */}
        </CarDetails>
      </CarCard>
    ))
  ) : (
    <p>No cars available to display. Please adjust your filters or try again later.</p>
  )}
</CarList>


      {/* Pagination */}
      <Pagination>
        <PaginationButton onClick={() => handlePageChange(currentPage - 1)} disabled={currentPage === 1}>
          Previous
        </PaginationButton>
        <span>{currentPage}</span>
        <PaginationButton
          onClick={() => handlePageChange(currentPage + 1)}
          disabled={currentPage * itemsPerPage >= sortedCars.length}
        >
          Next
        </PaginationButton>
      </Pagination>
    </Container>
  );
};

export default RoadReadyHome;
