# DriveHub - Car Rental Website

A modern, production-ready car rental platform built with Vue 3, TypeScript, and Tailwind CSS.

## Features Implemented

### Pages & Routes
- **Home Page** (`/`) - Hero section with search bar, featured cars, and CTA buttons
- **Car Listing Page** (`/cars`) - Browse all cars with advanced filters and sorting
- **Car Detail Page** (`/cars/:id`) - Full car information, specs, image gallery, availability check
- **Reservation Page** (`/reservation/:carId`) - 4-step booking flow with customer details, summary, payment
- **Confirmation Page** (`/confirmation/:reservationId`) - Order confirmation with reservation details

### Core Functionality

#### 1. Car Management
- 8 pre-loaded sample vehicles with realistic data
- Categories: Economy, Comfort, Premium, SUV, Luxury
- Full vehicle specifications: seats, luggage, transmission, fuel type, horsepower
- Multiple images per vehicle
- Price per day tracking

#### 2. Car Browsing & Discovery
- **Filters**: Category, Brand, Transmission, Fuel Type, Price Range
- **Sorting**: Price (Low-High, High-Low), Name (A-Z)
- **Search**: Quick search from home page
- Responsive grid layout (1-2 columns on mobile, 2 columns on tablet)
- Real-time filter updates with result counting

#### 3. Availability Management
- Date-based availability checking
- Blocked dates tracking for booked periods
- Availability verification before allowing reservations
- Prevents double-booking of vehicles

#### 4. Reservation Flow
- **Step 1**: Customer information (name, email, phone)
- **Step 2**: Reservation summary with price breakdown
- **Step 3**: Payment information collection (demo mode)
- **Step 4**: Final confirmation review
- Progress indicator showing current step
- Form validation at each step

#### 5. Pricing System
- Base: Daily rental rate × number of days
- Reservation fee: 15% of rental total
- Real-time total price calculation
- Clear price breakdown in summary and confirmation

#### 6. Navigation & UX
- Persistent header with navigation
- Footer with links and contact info
- Breadcrumb navigation on detail pages
- Responsive design for mobile, tablet, desktop
- Smooth transitions and hover states

## Technology Stack

- **Frontend Framework**: Vue 3.5.17 with Composition API
- **Language**: TypeScript 5
- **Styling**: Tailwind CSS 3.4.11
- **Routing**: Vue Router
- **Build Tool**: Vite 7
- **Package Manager**: npm
- **Testing**: Vitest (configured)
- **Linting**: ESLint 9

## Project Structure

```
src/
├── main.ts                 # App entry point with router config
├── App.vue                 # Root component with layout
├── types.ts                # TypeScript interfaces
├── pages/
│   ├── HomePage.vue        # Landing page
│   ├── CarsPage.vue        # Car listing & filtering
│   ├── CarDetailPage.vue   # Individual car details
│   ├── ReservationPage.vue # Multi-step booking form
│   └── ConfirmationPage.vue # Order confirmation
├── components/
│   ├── Header.vue          # Navigation header
│   ├── Footer.vue          # Footer with links
│   └── CarCard.vue         # Reusable car card component
├── stores/
│   └── cars.ts             # Car data & reservation management
└── assets/
    ├── main.css            # Global styles
    └── base.css            # Tailwind setup
```

## Key Features

### Design
- Clean, modern interface with professional styling
- Color scheme: Dark blue primary (#0f172a), Bright blue accent (#2563eb)
- Consistent spacing and typography
- Dark footer for visual separation
- Responsive layout across all breakpoints

### Data Models
- **Car**: Vehicle with specs, images, pricing, availability
- **Customer**: Renter information (name, email, phone)
- **Reservation**: Complete booking record with dates and pricing
- **Availability**: Date-based blocking system

### Logic
- Real-time availability checking prevents double bookings
- Automatic date blocking after successful reservation
- Form validation with user feedback
- Price calculation with fee percentage
- Persistent reservation storage

## How to Use

1. **Browse Cars**: Visit `/cars` to see all available vehicles with filters
2. **View Details**: Click on any car to see full details and check availability
3. **Check Availability**: Select pickup and return dates to check availability
4. **Make Reservation**: Click "Reserve Now" to start the 4-step booking process
5. **Complete Payment**: Fill in customer info, review summary, and complete demo payment
6. **Confirmation**: View your reservation confirmation with all details

## Demo Data

The app comes with 8 sample cars including:
- Tesla Model 3 (Premium Electric) - $120/day
- Honda Civic (Economy) - $50/day
- BMW X5 (SUV) - $180/day
- Mercedes-Benz C-Class (Luxury) - $200/day
- Toyota Prius (Comfort Hybrid) - $65/day
- Audi A4 (Premium) - $140/day
- Ford Mustang (Premium Sports) - $160/day
- Volkswagen Golf (Economy) - $55/day

## Future Enhancements

- User authentication and accounts
- Payment gateway integration (Stripe, PayPal)
- Email notifications for confirmations
- Admin dashboard for car management
- Rating and review system
- Multi-location support
- Advanced calendar view
- Cancellation/modification management
- Insurance upgrade options
