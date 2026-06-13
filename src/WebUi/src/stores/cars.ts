import { ref } from 'vue'
import type { Car, Reservation, ReservationRequest } from '../types'

export const cars = ref<Car[]>([])
export const carsLoading = ref(false)
export const carsError = ref<string | null>(null)

type CarsApiItem = {
  licensePlate?: string
  name?: string
  description?: string
  brand?: string
  model?: string
  category?: string
  pricePerDay?: string | number
  images?: string[]
  specifications?: {
    seats?: number
    luggage?: number | string
    transmission?: string
    fuelType?: string
    horsePower?: number | string
  }
  blockedDates?: string[]
  unavailableDates?: string[]
}

type CarsApiResponse = {
  items?: CarsApiItem[]
}

type RegisterCarCommand = {
  licensePlate: string
  name: string
  description: string
  brand: string
  model: string
  category: string
  pricePerDay: number | string
}

type ReservationApiItem = {
  id?: string
  reservationId?: string
  carId?: string
  licensePlate?: string
  customer?: {
    firstName?: string
    lastName?: string
    email?: string
    phone?: string
  }
  firstName?: string
  lastName?: string
  email?: string
  phone?: string
  startDate?: string
  endDate?: string
  pickupDate?: string
  returnDate?: string
  totalPrice?: string | number
  reservationFee?: string | number
  createdAt?: string
}

const categoryMap: Record<string, Car['category']> = {
  economy: 'economy',
  standard: 'comfort',
  comfort: 'comfort',
  premium: 'premium',
  suv: 'suv',
  luxury: 'luxury',
}

const fallbackCarImage =
  'https://images.unsplash.com/photo-1493238792000-8113da705763?w=800&h=600&fit=crop'

function readJsonSafely(text: string): unknown {
  if (!text.trim()) return null
  try {
    return JSON.parse(text)
  } catch {
    return null
  }
}

function parseNumber(value: string | number | undefined): number {
  if (typeof value === 'number') return Number.isFinite(value) ? value : 0
  if (typeof value !== 'string') return 0

  const numeric = Number(value.replace(/[^\d.,-]/g, '').replace(',', '.'))
  return Number.isFinite(numeric) ? numeric : 0
}

function parsePricePerDay(value: string | number | undefined): number {
  return parseNumber(value)
}

function normalizeCategory(value: string | undefined): Car['category'] {
  const key = value?.trim().toLowerCase() ?? ''
  return categoryMap[key] ?? 'economy'
}

const transmissionMap: Record<string, Car['specs']['transmission']> = {
  automatic: 'automatic',
  auto: 'automatic',
  manual: 'manual',
  mt: 'manual',
  at: 'automatic',
}

const fuelTypeMap: Record<string, Car['specs']['fuelType']> = {
  petrol: 'petrol',
  gasoline: 'petrol',
  gas: 'petrol',
  diesel: 'diesel',
  electric: 'electric',
  ev: 'electric',
  hybrid: 'hybrid',
}

function normalizeTransmission(value: string | undefined): Car['specs']['transmission'] {
  return transmissionMap[value?.trim().toLowerCase() ?? ''] ?? 'automatic'
}

function normalizeFuelType(value: string | undefined): Car['specs']['fuelType'] {
  return fuelTypeMap[value?.trim().toLowerCase() ?? ''] ?? 'petrol'
}

function mapApiCarToCar(apiCar: CarsApiItem): Car {
  const images = Array.isArray(apiCar.images) ? apiCar.images.filter(Boolean) : []

  return {
    id: apiCar.licensePlate?.trim() || `CAR-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`,
    name: apiCar.name?.trim() || 'Unknown car',
    description: apiCar.description?.trim() || '',
    brand: apiCar.brand?.trim() || 'Unknown',
    model: apiCar.model?.trim() || 'Unknown',
    category: normalizeCategory(apiCar.category),
    pricePerDay: parsePricePerDay(apiCar.pricePerDay),
    images: images.length > 0 ? images : [fallbackCarImage],
    specs: {
      seats: apiCar.specifications?.seats ?? 5,
      luggage: parseNumber(apiCar.specifications?.luggage) || 3,
      transmission: normalizeTransmission(apiCar.specifications?.transmission),
      fuelType: normalizeFuelType(apiCar.specifications?.fuelType),
      powerHP: parseNumber(apiCar.specifications?.horsePower) || 150,
    },
    availability: {
      blockedDates: Array.isArray(apiCar.blockedDates)
        ? apiCar.blockedDates
        : (apiCar.unavailableDates ?? []),
    },
  }
}

export async function fetchCars(page = 1, itemsPerPage = 100) {
  carsLoading.value = true
  carsError.value = null

  try {
    const response = await fetch(`/api/cars?page=${page}&itemsPerPage=${itemsPerPage}`)
    if (!response.ok) {
      throw new Error(`Failed to load cars: ${response.status}`)
    }

    const responseText = await response.text()
    const data = readJsonSafely(responseText) as CarsApiResponse | CarsApiItem[] | null
    const apiCars = Array.isArray(data) ? data : (data?.items ?? [])
    cars.value = apiCars.map(mapApiCarToCar)
  } catch (error) {
    carsError.value = error instanceof Error ? error.message : 'Failed to load cars'
    cars.value = []
  } finally {
    carsLoading.value = false
  }
}

export async function registerCar(command: RegisterCarCommand): Promise<boolean> {
  carsError.value = null

  try {
    const response = await fetch('/api/cars', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(command),
    })

    if (!response.ok) {
      throw new Error(`Failed to register car: ${response.status}`)
    }

    await fetchCars()
    return true
  } catch (error) {
    carsError.value = error instanceof Error ? error.message : 'Failed to register car'
    return false
  }
}

export const reservations = ref<Reservation[]>([])
export const reservationsLoading = ref(false)
export const reservationsError = ref<string | null>(null)

function upsertReservation(reservation: Reservation) {
  const index = reservations.value.findIndex((r) => r.id === reservation.id)
  if (index >= 0) {
    reservations.value[index] = reservation
  } else {
    reservations.value.push(reservation)
  }
}

function mapApiReservationToReservation(
  apiReservation: ReservationApiItem,
  fallback?: Partial<Reservation>,
): Reservation {
  const customer = apiReservation.customer ?? {}

  return {
    id:
      apiReservation.id?.toString() ||
      apiReservation.reservationId?.toString() ||
      fallback?.id ||
      `RES-${Date.now()}`,
    carId: apiReservation.carId || apiReservation.licensePlate || fallback?.carId || '',
    customer: {
      firstName: customer.firstName || apiReservation.firstName || fallback?.customer?.firstName || '',
      lastName: customer.lastName || apiReservation.lastName || fallback?.customer?.lastName || '',
      email: customer.email || apiReservation.email || fallback?.customer?.email || '',
      phone: customer.phone || apiReservation.phone || fallback?.customer?.phone || '',
    },
    startDate: apiReservation.startDate || apiReservation.pickupDate || fallback?.startDate || '',
    endDate: apiReservation.endDate || apiReservation.returnDate || fallback?.endDate || '',
    totalPrice:
      apiReservation.totalPrice !== undefined
        ? parseNumber(apiReservation.totalPrice)
        : (fallback?.totalPrice ?? 0),
    reservationFee:
      apiReservation.reservationFee !== undefined
        ? parseNumber(apiReservation.reservationFee)
        : (fallback?.reservationFee ?? 0),
    createdAt: apiReservation.createdAt || fallback?.createdAt || new Date().toISOString(),
  }
}

export function getCarById(id: string) {
  return cars.value.find((car) => car.id === id)
}

export function getReservationById(id: string) {
  return reservations.value.find((reservation) => reservation.id === id)
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

export async function createReservation(data: ReservationRequest): Promise<Reservation> {
  reservationsLoading.value = true
  reservationsError.value = null

  const fallbackReservation: Reservation = {
    id: `RES-${Date.now()}`,
    ...data,
    createdAt: new Date().toISOString(),
  }

  const requestBody = {
    ...data,
    licensePlate: data.carId,
    pickupDate: data.startDate,
    returnDate: data.endDate,
  }

  try {
    const response = await fetch('/api/reservations', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(requestBody),
    })

    if (!response.ok) {
      throw new Error(`Failed to create reservation: ${response.status}`)
    }

    const responseText = await response.text()
    const responseData = readJsonSafely(responseText)

    const reservation = responseData
      ? mapApiReservationToReservation(responseData as ReservationApiItem, fallbackReservation)
      : fallbackReservation

    upsertReservation(reservation)
    blockCarDates(reservation.carId, reservation.startDate, reservation.endDate)
    return reservation
  } catch (error) {
    reservationsError.value = error instanceof Error ? error.message : 'Failed to create reservation'
    throw error
  } finally {
    reservationsLoading.value = false
  }
}

export async function fetchReservationById(id: string): Promise<Reservation | undefined> {
  const cachedReservation = getReservationById(id)
  if (cachedReservation) return cachedReservation

  reservationsLoading.value = true
  reservationsError.value = null

  try {
    const response = await fetch(`/api/reservations/${encodeURIComponent(id)}`)
    if (!response.ok) {
      if (response.status === 404) return undefined
      throw new Error(`Failed to load reservation: ${response.status}`)
    }

    const responseText = await response.text()
    const responseData = readJsonSafely(responseText) as ReservationApiItem | null
    if (!responseData) return undefined

    const reservation = mapApiReservationToReservation(responseData, { id })
    upsertReservation(reservation)
    return reservation
  } catch (error) {
    reservationsError.value = error instanceof Error ? error.message : 'Failed to load reservation'
    return undefined
  } finally {
    reservationsLoading.value = false
  }
}

void fetchCars()
