import { ref } from 'vue'
import type { Car, Reservation } from '../types'

export const cars = ref<Car[]>([
  {
    id: '1',
    name: 'Tesla Model 3',
    description: 'Electric luxury sedan with advanced autopilot features and impressive range',
    brand: 'Tesla',
    model: 'Model 3',
    category: 'premium',
    pricePerDay: 120,
    images: [
      'https://images.unsplash.com/photo-1560958089-b8a46dd52915?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1550355291-bbee04a92027?w=800&h=600&fit=crop',
    ],
    specs: {
      seats: 5,
      luggage: 3,
      transmission: 'automatic',
      fuelType: 'electric',
      powerHP: 450,
    },
    availability: {
      blockedDates: [],
    },
  },
  {
    id: '2',
    name: 'Honda Civic',
    description: 'Reliable and efficient economy sedan, perfect for city driving',
    brand: 'Honda',
    model: 'Civic',
    category: 'economy',
    pricePerDay: 50,
    images: [
      'https://images.unsplash.com/photo-1590362891990-f8023379867c?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1552820728-8ac41f1ce891?w=800&h=600&fit=crop',
    ],
    specs: {
      seats: 5,
      luggage: 4,
      transmission: 'automatic',
      fuelType: 'petrol',
      powerHP: 158,
    },
    availability: {
      blockedDates: [],
    },
  },
  {
    id: '3',
    name: 'BMW X5',
    description: 'Premium SUV with luxurious interior and powerful performance',
    brand: 'BMW',
    model: 'X5',
    category: 'suv',
    pricePerDay: 180,
    images: [
      'https://images.unsplash.com/photo-1469854523086-cc02fe5d8800?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1517649763962-0c623066013b?w=800&h=600&fit=crop',
    ],
    specs: {
      seats: 7,
      luggage: 6,
      transmission: 'automatic',
      fuelType: 'diesel',
      powerHP: 381,
    },
    availability: {
      blockedDates: [],
    },
  },
  {
    id: '4',
    name: 'Mercedes-Benz C-Class',
    description: 'Elegant luxury sedan with cutting-edge technology and comfort',
    brand: 'Mercedes-Benz',
    model: 'C-Class',
    category: 'luxury',
    pricePerDay: 200,
    images: [
      'https://images.unsplash.com/photo-1553882900-d5160ca8dc08?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1618162692292-7ac56d7f7f1e?w=800&h=600&fit=crop',
    ],
    specs: {
      seats: 5,
      luggage: 4,
      transmission: 'automatic',
      fuelType: 'petrol',
      powerHP: 255,
    },
    availability: {
      blockedDates: [],
    },
  },
  {
    id: '5',
    name: 'Toyota Prius',
    description: 'Eco-friendly hybrid sedan with excellent fuel efficiency',
    brand: 'Toyota',
    model: 'Prius',
    category: 'comfort',
    pricePerDay: 65,
    images: [
      'https://images.unsplash.com/photo-1552519507-da3effff991c?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1614162692292-7ac56d7f7f1e?w=800&h=600&fit=crop',
    ],
    specs: {
      seats: 5,
      luggage: 4,
      transmission: 'automatic',
      fuelType: 'hybrid',
      powerHP: 121,
    },
    availability: {
      blockedDates: [],
    },
  },
  {
    id: '6',
    name: 'Audi A4',
    description: 'Premium sedan with sophisticated design and smooth driving experience',
    brand: 'Audi',
    model: 'A4',
    category: 'premium',
    pricePerDay: 140,
    images: [
      'https://images.unsplash.com/photo-1606664515524-2ddc6c2f0c45?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1495776050391-9d4ccdd180fa?w=800&h=600&fit=crop',
    ],
    specs: {
      seats: 5,
      luggage: 4,
      transmission: 'automatic',
      fuelType: 'petrol',
      powerHP: 245,
    },
    availability: {
      blockedDates: [],
    },
  },
  {
    id: '7',
    name: 'Ford Mustang',
    description: 'Iconic sports car with thrilling performance and striking design',
    brand: 'Ford',
    model: 'Mustang',
    category: 'premium',
    pricePerDay: 160,
    images: [
      'https://images.unsplash.com/photo-1637692234275-f8de86238ec8?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1567818735868-e71b99932e29?w=800&h=600&fit=crop',
    ],
    specs: {
      seats: 5,
      luggage: 3,
      transmission: 'automatic',
      fuelType: 'petrol',
      powerHP: 458,
    },
    availability: {
      blockedDates: [],
    },
  },
  {
    id: '8',
    name: 'Volkswagen Golf',
    description: 'Versatile hatchback combining practicality with German engineering',
    brand: 'Volkswagen',
    model: 'Golf',
    category: 'economy',
    pricePerDay: 55,
    images: [
      'https://images.unsplash.com/photo-1552519507-da3effff991c?w=800&h=600&fit=crop',
      'https://images.unsplash.com/photo-1567818735868-e71b99932e29?w=800&h=600&fit=crop',
    ],
    specs: {
      seats: 5,
      luggage: 5,
      transmission: 'automatic',
      fuelType: 'petrol',
      powerHP: 190,
    },
    availability: {
      blockedDates: [],
    },
  },
])

export const reservations = ref<Reservation[]>([])

export function getCarById(id: string) {
  return cars.value.find((car) => car.id === id)
}

export function getAvailableCars(startDate: string, endDate: string) {
  return cars.value.filter((car) => {
    const blockedDates = car.availability.blockedDates
    const start = new Date(startDate)
    const end = new Date(endDate)

    for (let d = new Date(start); d <= end; d.setDate(d.getDate() + 1)) {
      if (blockedDates.includes(d.toISOString().split('T')[0])) {
        return false
      }
    }
    return true
  })
}

export function blockCarDates(carId: string, startDate: string, endDate: string) {
  const car = getCarById(carId)
  if (!car) return

  const start = new Date(startDate)
  const end = new Date(endDate)

  for (let d = new Date(start); d <= end; d.setDate(d.getDate() + 1)) {
    const dateStr = d.toISOString().split('T')[0]
    if (!car.availability.blockedDates.includes(dateStr)) {
      car.availability.blockedDates.push(dateStr)
    }
  }
}

export function createReservation(data: any) {
  const reservation: Reservation = {
    id: `RES-${Date.now()}`,
    ...data,
    createdAt: new Date().toISOString(),
  }
  reservations.value.push(reservation)
  blockCarDates(data.carId, data.startDate, data.endDate)
  return reservation
}
