export interface CarSpecs {
  seats: number
  luggage: number
  transmission: 'manual' | 'automatic'
  fuelType: 'petrol' | 'diesel' | 'electric' | 'hybrid'
  powerHP: number
}

export interface Car {
  id: string
  name: string
  description: string
  brand: string
  model: string
  category: 'economy' | 'comfort' | 'premium' | 'suv' | 'luxury'
  pricePerDay: number
  images: string[]
  specs: CarSpecs
  availability: {
    blockedDates: string[] // ISO date strings
  }
}

export interface Customer {
  firstName: string
  lastName: string
  email: string
  phone: string
}

export interface ReservationRequest {
  carId: string
  customer: Customer
  startDate: string
  endDate: string
  totalPrice: number
  reservationFee: number
}

export interface Reservation {
  id: string
  carId: string
  customer: Customer
  startDate: string
  endDate: string
  totalPrice: number
  reservationFee: number
  createdAt: string
}
