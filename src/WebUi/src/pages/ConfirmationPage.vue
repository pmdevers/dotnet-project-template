<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'
import { fetchReservationById, getReservationById } from '../stores/cars'
import type { Reservation } from '../types'

const route = useRoute()
const reservation = ref<Reservation | null>(null)
const loading = ref(true)

const reservationId = computed(() => route.params.reservationId as string)

async function loadReservation() {
  loading.value = true
  const id = reservationId.value

  const cached = getReservationById(id)
  if (cached) {
    reservation.value = cached
    loading.value = false
    return
  }

  reservation.value = (await fetchReservationById(id)) ?? null
  loading.value = false
}

const formattedDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
  })
}

const printPage = () => window.print()

onMounted(() => {
  void loadReservation()
})
</script>

<template>
  <div v-if="loading" class="min-h-screen flex items-center justify-center">
    <p class="text-gray-600 text-lg">Loading reservation...</p>
  </div>

  <div class="min-h-screen bg-gradient-to-br from-green-50 to-green-100 py-12" v-if="reservation">
    <div class="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8">
      <!-- Success Card -->
      <div class="bg-white rounded-lg shadow-xl overflow-hidden">
        <!-- Header -->
        <div class="bg-gradient-to-r from-green-500 to-green-600 text-white p-8 text-center">
          <div class="text-6xl mb-4">✓</div>
          <h1 class="text-4xl font-bold mb-2">Reservation Confirmed!</h1>
          <p class="text-green-100 text-lg">Your booking has been successfully created</p>
        </div>

        <!-- Confirmation Number -->
        <div class="bg-green-50 border-b border-green-200 p-6">
          <div class="text-center">
            <p class="text-sm text-gray-600 mb-2">Confirmation Number</p>
            <p class="text-3xl font-mono font-bold text-gray-900">{{ reservation.id }}</p>
            <p class="text-sm text-gray-600 mt-2">A confirmation email has been sent to {{ reservation.customer.email }}</p>
          </div>
        </div>

        <!-- Reservation Details -->
        <div class="p-8">
          <h2 class="text-2xl font-bold text-primary mb-6">Reservation Details</h2>

          <div class="grid grid-cols-1 md:grid-cols-2 gap-8 mb-8">
            <!-- Customer Information -->
            <div>
              <h3 class="text-lg font-semibold text-gray-900 mb-4">Customer Information</h3>
              <div class="space-y-3">
                <div>
                  <p class="text-sm text-gray-600">Name</p>
                  <p class="font-semibold text-gray-900">
                    {{ reservation.customer.firstName }} {{ reservation.customer.lastName }}
                  </p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Email</p>
                  <p class="font-semibold text-gray-900">{{ reservation.customer.email }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Phone</p>
                  <p class="font-semibold text-gray-900">{{ reservation.customer.phone }}</p>
                </div>
              </div>
            </div>

            <!-- Rental Period -->
            <div>
              <h3 class="text-lg font-semibold text-gray-900 mb-4">Rental Period</h3>
              <div class="space-y-3">
                <div>
                  <p class="text-sm text-gray-600">Pickup Date & Time</p>
                  <p class="font-semibold text-gray-900">{{ formattedDate(reservation.startDate) }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Return Date & Time</p>
                  <p class="font-semibold text-gray-900">{{ formattedDate(reservation.endDate) }}</p>
                </div>
                <div>
                  <p class="text-sm text-gray-600">Duration</p>
                  <p class="font-semibold text-gray-900">
                    {{
                      Math.ceil(
                        (new Date(reservation.endDate).getTime() - new Date(reservation.startDate).getTime()) /
                          (1000 * 60 * 60 * 24),
                      )
                    }}
                    days
                  </p>
                </div>
              </div>
            </div>
          </div>

          <!-- Payment Summary -->
          <div class="border-t border-gray-200 pt-8 mb-8">
            <h3 class="text-lg font-semibold text-gray-900 mb-4">Payment Summary</h3>

            <div class="bg-gray-50 rounded-lg p-6">
              <div class="space-y-3">
                <div class="flex justify-between text-gray-700">
                  <span>Vehicle Rental</span>
                  <span class="font-semibold">${{ reservation.totalPrice }}</span>
                </div>
                <div class="flex justify-between text-gray-700">
                  <span>Reservation Fee</span>
                  <span class="font-semibold">${{ reservation.reservationFee }}</span>
                </div>
                <div class="flex justify-between text-xl font-bold text-accent pt-3 border-t border-gray-300">
                  <span>Total Paid</span>
                  <span>${{ reservation.totalPrice + reservation.reservationFee }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- What's Included -->
          <div class="bg-blue-50 rounded-lg p-6 border border-blue-200">
            <h3 class="font-semibold text-blue-900 mb-4">What's Included</h3>
            <ul class="space-y-2 text-blue-800">
              <li class="flex items-center gap-2">
                <svg class="w-5 h-5 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                </svg>
                Full insurance coverage
              </li>
              <li class="flex items-center gap-2">
                <svg class="w-5 h-5 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                </svg>
                Unlimited mileage
              </li>
              <li class="flex items-center gap-2">
                <svg class="w-5 h-5 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                </svg>
                Free cancellation up to 24 hours
              </li>
              <li class="flex items-center gap-2">
                <svg class="w-5 h-5 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                </svg>
                24/7 roadside assistance
              </li>
              <li class="flex items-center gap-2">
                <svg class="w-5 h-5 flex-shrink-0" fill="currentColor" viewBox="0 0 20 20">
                  <path fill-rule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clip-rule="evenodd" />
                </svg>
                Free parking
              </li>
            </ul>
          </div>

          <!-- Important Notes -->
          <div class="mt-8 p-6 bg-amber-50 border border-amber-200 rounded-lg">
            <h3 class="font-semibold text-amber-900 mb-3">Important Information</h3>
            <ul class="text-sm text-amber-800 space-y-2">
              <li>
                ✓ Please arrive 15 minutes before your pickup time with a valid driver's license and credit card
              </li>
              <li>✓ Return the car to the same location on the agreed date and time</li>
              <li>
                ✓ You will be responsible for any damages beyond normal wear and tear. A damage inspection will be
                conducted
              </li>
              <li>✓ Contact us if you need to cancel or modify your reservation</li>
            </ul>
          </div>
        </div>

        <!-- Action Buttons -->
        <div class="bg-gray-50 border-t border-gray-200 p-8 flex flex-col sm:flex-row gap-4">
          <button
            @click="printPage"
            class="px-6 py-3 border border-gray-300 rounded-lg font-medium text-gray-700 hover:bg-gray-100 transition-colors"
          >
            Print Confirmation
          </button>
          <router-link
            to="/"
            class="px-6 py-3 bg-accent hover:bg-accent-dark text-white font-semibold rounded-lg transition-colors text-center"
          >
            Return to Home
          </router-link>
        </div>
      </div>

      <!-- Contact Support -->
      <div class="text-center mt-12">
        <p class="text-gray-600 mb-4">Need help?</p>
        <div class="flex flex-col sm:flex-row items-center justify-center gap-6 text-sm text-gray-700">
          <div>📧 support@drivehub.com</div>
          <div>📞 1-800-DRIVE-HUB</div>
          <div>💬 Live chat available 24/7</div>
        </div>
      </div>
    </div>
  </div>

  <!-- Not Found -->
  <div v-else-if="!loading" class="min-h-screen flex items-center justify-center">
    <div class="text-center">
      <p class="text-gray-600 mb-4 text-lg">Reservation not found</p>
      <router-link to="/" class="text-accent hover:text-accent-dark font-medium">
        Return to Home
      </router-link>
    </div>
  </div>
</template>
