<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { getCarById, createReservation } from '../stores/cars'

const route = useRoute()
const router = useRouter()

const car = computed(() => getCarById(route.params.carId as string))

const startDate = computed(() => route.query.startDate as string)
const endDate = computed(() => route.query.endDate as string)

const step = ref(1)

// Form data
const customerData = ref({
  firstName: '',
  lastName: '',
  email: '',
  phone: '',
})

const paymentData = ref({
  cardNumber: '',
  expiry: '',
  cvc: '',
  cardholderName: '',
})

const calculateDays = computed(() => {
  if (!startDate.value || !endDate.value) return 0
  const start = new Date(startDate.value)
  const end = new Date(endDate.value)
  return Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24))
})

const reservationFeePercentage = 0.15 // 15% fee

const rentalTotal = computed(() => {
  if (!car.value) return 0
  return car.value.pricePerDay * calculateDays.value
})

const reservationFee = computed(() => {
  return Math.round(rentalTotal.value * reservationFeePercentage * 100) / 100
})

const totalPrice = computed(() => {
  return rentalTotal.value + reservationFee.value
})

function validateStep1() {
  if (!customerData.value.firstName.trim()) {
    alert('Please enter first name')
    return false
  }
  if (!customerData.value.lastName.trim()) {
    alert('Please enter last name')
    return false
  }
  if (!customerData.value.email.trim() || !customerData.value.email.includes('@')) {
    alert('Please enter valid email')
    return false
  }
  if (!customerData.value.phone.trim()) {
    alert('Please enter phone number')
    return false
  }
  return true
}

function validateStep3() {
  if (!paymentData.value.cardNumber.replace(/\s/g, '').length === 16) {
    alert('Please enter valid card number')
    return false
  }
  if (!paymentData.value.expiry.trim()) {
    alert('Please enter expiry date')
    return false
  }
  if (!paymentData.value.cvc.trim() || paymentData.value.cvc.length < 3) {
    alert('Please enter valid CVC')
    return false
  }
  if (!paymentData.value.cardholderName.trim()) {
    alert('Please enter cardholder name')
    return false
  }
  return true
}

function nextStep() {
  if (step.value === 1 && !validateStep1()) return
  if (step.value === 3 && !validateStep3()) return

  if (step.value < 4) {
    step.value++
  }
}

function prevStep() {
  if (step.value > 1) {
    step.value--
  }
}

function completeReservation() {
  if (!car.value) return

  const reservation = createReservation({
    carId: car.value.id,
    customer: customerData.value,
    startDate: startDate.value,
    endDate: endDate.value,
    totalPrice: rentalTotal.value,
    reservationFee: reservationFee.value,
  })

  router.push({
    path: `/confirmation/${reservation.id}`,
  })
}

function formatCardNumber(value: string) {
  return value
    .replace(/\s/g, '')
    .replace(/(\d{4})/g, '$1 ')
    .trim()
}
</script>

<template>
  <div class="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100 py-8" v-if="car">
    <div class="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8">
      <!-- Header -->
      <div class="mb-8">
        <h1 class="text-4xl font-bold text-primary mb-2">Complete Your Reservation</h1>
        <p class="text-gray-600">Secure booking for {{ car.name }} from {{ startDate }} to {{ endDate }}</p>
      </div>

      <!-- Progress Steps -->
      <div class="flex items-center justify-between mb-8">
        <div v-for="s in 4" :key="s" class="flex items-center">
          <div
            :class="[
              'w-10 h-10 rounded-full font-bold flex items-center justify-center transition-colors',
              step >= s ? 'bg-accent text-white' : 'bg-gray-300 text-gray-600',
            ]"
          >
            {{ s }}
          </div>
          <div
            v-if="s < 4"
            :class="[
              'w-12 h-1 mx-2 transition-colors',
              step > s ? 'bg-accent' : 'bg-gray-300',
            ]"
          ></div>
        </div>
      </div>

      <!-- Step Labels -->
      <div class="grid grid-cols-4 gap-2 mb-8 text-center">
        <div class="text-sm">
          <p :class="step >= 1 ? 'text-accent font-bold' : 'text-gray-600'">Details</p>
        </div>
        <div class="text-sm">
          <p :class="step >= 2 ? 'text-accent font-bold' : 'text-gray-600'">Summary</p>
        </div>
        <div class="text-sm">
          <p :class="step >= 3 ? 'text-accent font-bold' : 'text-gray-600'">Payment</p>
        </div>
        <div class="text-sm">
          <p :class="step >= 4 ? 'text-accent font-bold' : 'text-gray-600'">Confirm</p>
        </div>
      </div>

      <!-- Form Container -->
      <div class="bg-white rounded-lg shadow-lg p-8 mb-8">
        <!-- Step 1: Customer Details -->
        <div v-if="step === 1">
          <h2 class="text-2xl font-bold text-primary mb-6">Your Information</h2>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">First Name</label>
              <input
                v-model="customerData.firstName"
                type="text"
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent"
                placeholder="John"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Last Name</label>
              <input
                v-model="customerData.lastName"
                type="text"
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent"
                placeholder="Doe"
              />
            </div>

            <div class="md:col-span-2">
              <label class="block text-sm font-medium text-gray-700 mb-2">Email</label>
              <input
                v-model="customerData.email"
                type="email"
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent"
                placeholder="john@example.com"
              />
            </div>

            <div class="md:col-span-2">
              <label class="block text-sm font-medium text-gray-700 mb-2">Phone Number</label>
              <input
                v-model="customerData.phone"
                type="tel"
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent"
                placeholder="+1 (555) 123-4567"
              />
            </div>
          </div>
        </div>

        <!-- Step 2: Reservation Summary -->
        <div v-if="step === 2">
          <h2 class="text-2xl font-bold text-primary mb-6">Reservation Summary</h2>

          <div class="bg-gray-50 rounded-lg p-6 mb-6">
            <!-- Car Info -->
            <div class="flex gap-4 mb-6">
              <img :src="car.images[0]" :alt="car.name" class="w-24 h-24 rounded-lg object-cover" />
              <div class="flex-1">
                <h3 class="text-xl font-bold text-primary">{{ car.name }}</h3>
                <p class="text-gray-600">{{ car.description }}</p>
              </div>
            </div>

            <!-- Dates -->
            <div class="grid grid-cols-2 gap-4 mb-6 pb-6 border-b border-gray-200">
              <div>
                <p class="text-sm text-gray-600 mb-1">Pickup Date</p>
                <p class="font-bold text-gray-900">{{ startDate }}</p>
              </div>
              <div>
                <p class="text-sm text-gray-600 mb-1">Return Date</p>
                <p class="font-bold text-gray-900">{{ endDate }}</p>
              </div>
            </div>

            <!-- Customer Info -->
            <div class="mb-6 pb-6 border-b border-gray-200">
              <p class="text-sm text-gray-600 mb-2">Renter</p>
              <p class="font-bold text-gray-900">{{ customerData.firstName }} {{ customerData.lastName }}</p>
              <p class="text-sm text-gray-600">{{ customerData.email }}</p>
              <p class="text-sm text-gray-600">{{ customerData.phone }}</p>
            </div>

            <!-- Price Breakdown -->
            <div class="space-y-3">
              <div class="flex justify-between text-gray-700">
                <span>{{ calculateDays }} days × ${{ car.pricePerDay }}/day</span>
                <span class="font-semibold">${{ rentalTotal }}</span>
              </div>
              <div class="flex justify-between text-gray-700">
                <span>Reservation fee (15%)</span>
                <span class="font-semibold">${{ reservationFee }}</span>
              </div>
              <div class="flex justify-between text-xl font-bold text-accent pt-3 border-t border-gray-300">
                <span>Total Amount</span>
                <span>${{ totalPrice }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Step 3: Payment -->
        <div v-if="step === 3">
          <h2 class="text-2xl font-bold text-primary mb-6">Payment Information</h2>

          <div class="space-y-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Cardholder Name</label>
              <input
                v-model="paymentData.cardholderName"
                type="text"
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent"
                placeholder="John Doe"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 mb-2">Card Number</label>
              <input
                v-model="paymentData.cardNumber"
                type="text"
                maxlength="19"
                @input="paymentData.cardNumber = formatCardNumber(paymentData.cardNumber)"
                class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent font-mono"
                placeholder="1234 5678 9012 3456"
              />
            </div>

            <div class="grid grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Expiry Date</label>
                <input
                  v-model="paymentData.expiry"
                  type="text"
                  maxlength="5"
                  placeholder="MM/YY"
                  class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent"
                />
              </div>
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">CVC</label>
                <input
                  v-model="paymentData.cvc"
                  type="text"
                  maxlength="4"
                  placeholder="123"
                  class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent"
                />
              </div>
            </div>

            <div class="bg-blue-50 border border-blue-200 rounded-lg p-4">
              <p class="text-sm text-blue-900">
                💳 This is a demo. Use test card <strong>4111 1111 1111 1111</strong> to proceed.
              </p>
            </div>
          </div>
        </div>

        <!-- Step 4: Confirmation -->
        <div v-if="step === 4">
          <div class="text-center py-12">
            <div class="text-6xl mb-6">📋</div>
            <h2 class="text-3xl font-bold text-primary mb-4">Ready to Confirm?</h2>
            <p class="text-gray-600 mb-8 text-lg">
              Review your reservation details below and confirm to complete your booking.
            </p>

            <div class="bg-gray-50 rounded-lg p-8 text-left max-w-md mx-auto mb-8">
              <div class="space-y-4">
                <div>
                  <p class="text-sm text-gray-600">Vehicle</p>
                  <p class="font-bold text-gray-900">{{ car.name }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Duration</p>
                  <p class="font-bold text-gray-900">{{ calculateDays }} days</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Total Cost</p>
                  <p class="text-2xl font-bold text-accent">${{ totalPrice }}</p>
                </div>
                <div class="pt-4 border-t border-gray-300">
                  <p class="text-sm text-gray-600">Confirmation Email</p>
                  <p class="font-bold text-gray-900">{{ customerData.email }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Action Buttons -->
      <div class="flex items-center justify-between">
        <button
          @click="prevStep"
          v-if="step > 1"
          class="px-6 py-3 border border-gray-300 rounded-lg font-medium text-gray-700 hover:bg-gray-50 transition-colors"
        >
          Previous
        </button>

        <div></div>

        <button
          @click="step === 4 ? completeReservation() : nextStep()"
          class="px-8 py-3 bg-accent hover:bg-accent-dark text-white font-semibold rounded-lg transition-colors"
        >
          {{ step === 4 ? 'Confirm Reservation' : 'Next Step' }}
        </button>
      </div>
    </div>
  </div>
</template>
