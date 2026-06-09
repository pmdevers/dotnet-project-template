<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { getCarById } from '../stores/cars'

const route = useRoute()
const router = useRouter()

const car = computed(() => getCarById(route.params.id as string))

const selectedImageIndex = ref(0)
const startDate = ref('')
const endDate = ref('')
const availability = ref<boolean | null>(null)

function checkAvailability() {
  if (!startDate.value || !endDate.value) {
    alert('Please select both dates')
    return
  }

  const start = new Date(startDate.value)
  const end = new Date(endDate.value)

  if (start >= end) {
    alert('Return date must be after pickup date')
    return
  }

  if (!car.value) return

  const blockedDates = car.value.availability.blockedDates
  let isAvailable = true

  for (let d = new Date(start); d < end; d.setDate(d.getDate() + 1)) {
    if (blockedDates.includes(d.toISOString().split('T')[0])) {
      isAvailable = false
      break
    }
  }

  availability.value = isAvailable
}

function proceedToReservation() {
  if (!availability.value) {
    alert('Car is not available for selected dates')
    return
  }

  router.push({
    path: `/reservation/${car.value?.id}`,
    query: { startDate: startDate.value, endDate: endDate.value },
  })
}

const calculateDays = computed(() => {
  if (!startDate.value || !endDate.value) return 0
  const start = new Date(startDate.value)
  const end = new Date(endDate.value)
  return Math.ceil((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24))
})

const totalPrice = computed(() => {
  if (!car.value) return 0
  return car.value.pricePerDay * calculateDays.value
})
</script>

<template>
  <div class="min-h-screen bg-gray-50" v-if="car">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Breadcrumb -->
      <div class="flex items-center gap-2 mb-8 text-sm">
        <router-link to="/cars" class="text-accent hover:text-accent-dark">Cars</router-link>
        <span class="text-gray-400">/</span>
        <span class="text-gray-600">{{ car.name }}</span>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <!-- Image Gallery -->
        <div class="lg:col-span-2">
          <div class="bg-white rounded-lg overflow-hidden shadow-md mb-4">
            <div class="h-96 bg-gray-200 relative">
              <img
                :src="car.images[selectedImageIndex]"
                :alt="car.name"
                class="w-full h-full object-cover"
              />
            </div>

            <!-- Thumbnail Gallery -->
            <div class="flex gap-2 p-4 overflow-x-auto">
              <button
                v-for="(image, index) in car.images"
                :key="index"
                @click="selectedImageIndex = index"
                :class="[
                  'flex-shrink-0 w-20 h-20 rounded-lg overflow-hidden border-2 transition-colors',
                  selectedImageIndex === index ? 'border-accent' : 'border-gray-300 hover:border-gray-400',
                ]"
              >
                <img :src="image" :alt="`${car.name} ${index + 1}`" class="w-full h-full object-cover" />
              </button>
            </div>
          </div>

          <!-- Specifications -->
          <div class="bg-white rounded-lg shadow-md p-6">
            <h2 class="text-2xl font-bold text-primary mb-6">Specifications</h2>

            <div class="grid grid-cols-2 md:grid-cols-3 gap-6">
              <div class="text-center p-4 bg-gray-50 rounded-lg">
                <div class="text-3xl mb-2">👥</div>
                <p class="text-gray-600 text-sm">Seats</p>
                <p class="text-xl font-bold text-primary">{{ car.specs.seats }}</p>
              </div>

              <div class="text-center p-4 bg-gray-50 rounded-lg">
                <div class="text-3xl mb-2">🧳</div>
                <p class="text-gray-600 text-sm">Luggage</p>
                <p class="text-xl font-bold text-primary">{{ car.specs.luggage }}</p>
              </div>

              <div class="text-center p-4 bg-gray-50 rounded-lg">
                <div class="text-3xl mb-2">⚙️</div>
                <p class="text-gray-600 text-sm">Transmission</p>
                <p class="text-xl font-bold text-primary capitalize">{{ car.specs.transmission }}</p>
              </div>

              <div class="text-center p-4 bg-gray-50 rounded-lg">
                <div class="text-3xl mb-2">⛽</div>
                <p class="text-gray-600 text-sm">Fuel Type</p>
                <p class="text-xl font-bold text-primary capitalize">{{ car.specs.fuelType }}</p>
              </div>

              <div class="text-center p-4 bg-gray-50 rounded-lg">
                <div class="text-3xl mb-2">🏎️</div>
                <p class="text-gray-600 text-sm">Power</p>
                <p class="text-xl font-bold text-primary">{{ car.specs.powerHP }} HP</p>
              </div>

              <div class="text-center p-4 bg-gray-50 rounded-lg">
                <div class="text-3xl mb-2">📦</div>
                <p class="text-gray-600 text-sm">Category</p>
                <p class="text-xl font-bold text-primary capitalize">{{ car.category }}</p>
              </div>
            </div>

            <!-- Description -->
            <div class="mt-8 pt-8 border-t border-gray-200">
              <h3 class="text-lg font-bold text-primary mb-3">About This Vehicle</h3>
              <p class="text-gray-700 leading-relaxed">{{ car.description }}</p>
            </div>
          </div>
        </div>

        <!-- Booking Sidebar -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-lg shadow-md p-6 sticky top-20">
            <!-- Price -->
            <div class="mb-8">
              <div class="flex items-baseline gap-2">
                <span class="text-4xl font-bold text-accent">${{ car.pricePerDay }}</span>
                <span class="text-gray-600">/day</span>
              </div>
            </div>

            <!-- Date Pickers -->
            <div class="space-y-4 mb-6">
              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Pickup Date</label>
                <input
                  v-model="startDate"
                  type="date"
                  class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent"
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 mb-2">Return Date</label>
                <input
                  v-model="endDate"
                  type="date"
                  class="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent"
                />
              </div>
            </div>

            <!-- Check Availability Button -->
            <button
              @click="checkAvailability"
              class="w-full px-4 py-3 bg-gray-200 hover:bg-gray-300 text-gray-900 font-semibold rounded-lg transition-colors mb-4"
            >
              Check Availability
            </button>

            <!-- Availability Status -->
            <div v-if="availability !== null" class="mb-6 p-4 rounded-lg" :class="availability ? 'bg-green-50 border border-green-300' : 'bg-red-50 border border-red-300'">
              <p :class="availability ? 'text-green-800' : 'text-red-800'" class="font-medium">
                {{ availability ? '✓ Available' : '✗ Not Available' }}
              </p>
            </div>

            <!-- Price Calculation -->
            <div v-if="calculateDays > 0" class="bg-gray-50 rounded-lg p-4 mb-6">
              <div class="flex justify-between mb-2 text-sm">
                <span class="text-gray-600">{{ calculateDays }} days × ${{ car.pricePerDay }}</span>
                <span class="font-medium text-gray-900">${{ totalPrice }}</span>
              </div>
              <div class="border-t border-gray-200 pt-2 mt-2">
                <div class="flex justify-between">
                  <span class="font-bold text-gray-900">Total</span>
                  <span class="text-xl font-bold text-accent">${{ totalPrice }}</span>
                </div>
              </div>
            </div>

            <!-- Reserve Button -->
            <button
              @click="proceedToReservation"
              :disabled="!availability || calculateDays === 0"
              :class="[
                'w-full px-4 py-3 font-semibold rounded-lg transition-colors',
                availability && calculateDays > 0
                  ? 'bg-accent hover:bg-accent-dark text-white'
                  : 'bg-gray-300 text-gray-500 cursor-not-allowed',
              ]"
            >
              {{ calculateDays === 0 ? 'Select Dates' : 'Reserve Now' }}
            </button>

            <!-- Features -->
            <div class="mt-6 pt-6 border-t border-gray-200 space-y-3">
              <div class="flex items-start gap-3">
                <svg class="w-5 h-5 text-accent flex-shrink-0 mt-0.5" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                </svg>
                <span class="text-sm text-gray-700">Free cancellation up to 24 hours</span>
              </div>

              <div class="flex items-start gap-3">
                <svg class="w-5 h-5 text-accent flex-shrink-0 mt-0.5" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                </svg>
                <span class="text-sm text-gray-700">Insurance included</span>
              </div>

              <div class="flex items-start gap-3">
                <svg class="w-5 h-5 text-accent flex-shrink-0 mt-0.5" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                </svg>
                <span class="text-sm text-gray-700">24/7 roadside assistance</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div v-else class="min-h-screen flex items-center justify-center">
    <div class="text-center">
      <p class="text-gray-600 mb-4">Car not found</p>
      <router-link to="/cars" class="text-accent hover:text-accent-dark font-medium">
        Back to Cars
      </router-link>
    </div>
  </div>
</template>
