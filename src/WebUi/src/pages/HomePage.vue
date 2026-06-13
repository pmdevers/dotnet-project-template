<script setup lang="ts">
import { computed, ref } from 'vue'
import { useRouter } from 'vue-router'
import CarCard from '../components/CarCard.vue'
import { cars } from '../stores/cars'

const router = useRouter()

const searchForm = ref({
  pickupDate: '',
  returnDate: '',
  carType: 'all',
})

const featuredCars = computed(() => cars.value.slice(0, 3))

function handleSearch() {
  router.push('/cars')
}
</script>

<template>
  <div>
    <!-- Hero Section -->
    <section class="relative bg-gradient-to-r from-primary to-gray-900 text-white py-20">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="text-center mb-12">
          <h1 class="text-5xl md:text-6xl font-bold mb-4">Your Journey Starts Here</h1>
          <p class="text-xl text-gray-300">Find and book the perfect car for your next adventure</p>
        </div>

        <!-- Search Bar -->
        <div class="bg-white rounded-xl shadow-2xl p-8 grid grid-cols-1 md:grid-cols-4 gap-4">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">Pickup Date</label>
            <input
              v-model="searchForm.pickupDate"
              type="date"
              class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent text-gray-900"
            />
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">Return Date</label>
            <input
              v-model="searchForm.returnDate"
              type="date"
              class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent text-gray-900"
            />
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">Car Type</label>
            <select
              v-model="searchForm.carType"
              class="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent text-gray-900"
            >
              <option value="all">All Types</option>
              <option value="economy">Economy</option>
              <option value="comfort">Comfort</option>
              <option value="premium">Premium</option>
              <option value="suv">SUV</option>
              <option value="luxury">Luxury</option>
            </select>
          </div>

          <div class="flex items-end">
            <button
              @click="handleSearch"
              class="w-full px-6 py-3 bg-accent hover:bg-accent-dark text-white font-semibold rounded-lg transition-colors"
            >
              Search Cars
            </button>
          </div>
        </div>
      </div>
    </section>

    <!-- Featured Cars -->
    <section class="py-16 bg-gray-50">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div class="text-center mb-12">
          <h2 class="text-4xl font-bold text-primary mb-4">Featured Vehicles</h2>
          <p class="text-xl text-gray-600">Handpicked selection of our most popular cars</p>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
          <CarCard v-for="car in featuredCars" :key="car.id" :car="car" />
        </div>

        <div class="text-center mt-12">
          <router-link
            to="/cars"
            class="inline-block px-8 py-4 bg-accent hover:bg-accent-dark text-white font-semibold rounded-lg transition-colors"
          >
            Browse All Cars
          </router-link>
        </div>
      </div>
    </section>

    <!-- CTA Section -->
    <section class="py-16 bg-accent text-white">
      <div class="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
        <h2 class="text-4xl font-bold mb-4">Why Choose DriveHub?</h2>
        <p class="text-lg mb-8 text-blue-100">Experience hassle-free car rentals with competitive pricing and exceptional service</p>

        <div class="grid grid-cols-1 md:grid-cols-3 gap-8 mt-12">
          <div class="bg-blue-600 rounded-lg p-8">
            <div class="text-4xl mb-4">🚗</div>
            <h3 class="text-xl font-bold mb-2">Wide Selection</h3>
            <p class="text-blue-100">Choose from 100+ vehicles in various categories</p>
          </div>

          <div class="bg-blue-600 rounded-lg p-8">
            <div class="text-4xl mb-4">💰</div>
            <h3 class="text-xl font-bold mb-2">Best Prices</h3>
            <p class="text-blue-100">Competitive rates with transparent pricing</p>
          </div>

          <div class="bg-blue-600 rounded-lg p-8">
            <div class="text-4xl mb-4">✅</div>
            <h3 class="text-xl font-bold mb-2">Easy Booking</h3>
            <p class="text-blue-100">Simple and fast reservation process</p>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>
