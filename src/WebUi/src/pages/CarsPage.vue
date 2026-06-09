<script setup lang="ts">
import { ref, computed } from 'vue'
import CarCard from '../components/CarCard.vue'
import { cars } from '../stores/cars'

const filters = ref({
  category: 'all',
  brand: 'all',
  transmission: 'all',
  fuelType: 'all',
  priceMin: 0,
  priceMax: 200,
})

const sortBy = ref('price')

const uniqueBrands = computed(() => {
  const brands = new Set(cars.value.map((c) => c.brand))
  return Array.from(brands).sort()
})

const filteredAndSortedCars = computed(() => {
  let result = cars.value.filter((car) => {
    if (filters.value.category !== 'all' && car.category !== filters.value.category) return false
    if (filters.value.brand !== 'all' && car.brand !== filters.value.brand) return false
    if (filters.value.transmission !== 'all' && car.specs.transmission !== filters.value.transmission)
      return false
    if (filters.value.fuelType !== 'all' && car.specs.fuelType !== filters.value.fuelType) return false
    if (car.pricePerDay < filters.value.priceMin || car.pricePerDay > filters.value.priceMax) return false

    return true
  })

  // Sort
  if (sortBy.value === 'price-asc') {
    result = result.sort((a, b) => a.pricePerDay - b.pricePerDay)
  } else if (sortBy.value === 'price-desc') {
    result = result.sort((a, b) => b.pricePerDay - a.pricePerDay)
  } else if (sortBy.value === 'name') {
    result = result.sort((a, b) => a.name.localeCompare(b.name))
  }

  return result
})

function resetFilters() {
  filters.value = {
    category: 'all',
    brand: 'all',
    transmission: 'all',
    fuelType: 'all',
    priceMin: 0,
    priceMax: 200,
  }
  sortBy.value = 'price'
}
</script>

<template>
  <div class="min-h-screen bg-gray-50">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <!-- Header -->
      <div class="mb-8">
        <h1 class="text-4xl font-bold text-primary mb-2">Our Fleet</h1>
        <p class="text-gray-600">Browse and filter our selection of quality vehicles</p>
      </div>

      <div class="grid grid-cols-1 lg:grid-cols-4 gap-8">
        <!-- Filters Sidebar -->
        <div class="lg:col-span-1">
          <div class="bg-white rounded-lg shadow-md p-6 sticky top-20">
            <div class="flex items-center justify-between mb-6">
              <h2 class="text-lg font-bold text-primary">Filters</h2>
              <button
                @click="resetFilters"
                class="text-sm text-accent hover:text-accent-dark font-medium"
              >
                Reset
              </button>
            </div>

            <!-- Category Filter -->
            <div class="mb-6">
              <h3 class="font-semibold text-gray-900 mb-3">Category</h3>
              <div class="space-y-2">
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.category"
                    type="radio"
                    value="all"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">All</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.category"
                    type="radio"
                    value="economy"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Economy</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.category"
                    type="radio"
                    value="comfort"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Comfort</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.category"
                    type="radio"
                    value="premium"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Premium</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.category"
                    type="radio"
                    value="suv"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">SUV</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.category"
                    type="radio"
                    value="luxury"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Luxury</span>
                </label>
              </div>
            </div>

            <div class="border-t border-gray-200 pt-6 mb-6">
              <h3 class="font-semibold text-gray-900 mb-3">Brand</h3>
              <select
                v-model="filters.brand"
                class="w-full px-3 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent text-sm"
              >
                <option value="all">All Brands</option>
                <option v-for="brand in uniqueBrands" :key="brand" :value="brand">
                  {{ brand }}
                </option>
              </select>
            </div>

            <!-- Transmission Filter -->
            <div class="border-t border-gray-200 pt-6 mb-6">
              <h3 class="font-semibold text-gray-900 mb-3">Transmission</h3>
              <div class="space-y-2">
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.transmission"
                    type="radio"
                    value="all"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">All</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.transmission"
                    type="radio"
                    value="automatic"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Automatic</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.transmission"
                    type="radio"
                    value="manual"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Manual</span>
                </label>
              </div>
            </div>

            <!-- Fuel Type Filter -->
            <div class="border-t border-gray-200 pt-6 mb-6">
              <h3 class="font-semibold text-gray-900 mb-3">Fuel Type</h3>
              <div class="space-y-2">
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.fuelType"
                    type="radio"
                    value="all"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">All</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.fuelType"
                    type="radio"
                    value="petrol"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Petrol</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.fuelType"
                    type="radio"
                    value="diesel"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Diesel</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.fuelType"
                    type="radio"
                    value="hybrid"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Hybrid</span>
                </label>
                <label class="flex items-center cursor-pointer">
                  <input
                    v-model="filters.fuelType"
                    type="radio"
                    value="electric"
                    class="w-4 h-4 text-accent"
                  />
                  <span class="ml-3 text-gray-700">Electric</span>
                </label>
              </div>
            </div>

            <!-- Price Range Filter -->
            <div class="border-t border-gray-200 pt-6">
              <h3 class="font-semibold text-gray-900 mb-4">Price Range</h3>
              <div class="space-y-3">
                <div>
                  <label class="text-sm text-gray-600">Min: ${{ filters.priceMin }}</label>
                  <input
                    v-model.number="filters.priceMin"
                    type="range"
                    min="0"
                    max="200"
                    class="w-full"
                  />
                </div>
                <div>
                  <label class="text-sm text-gray-600">Max: ${{ filters.priceMax }}</label>
                  <input
                    v-model.number="filters.priceMax"
                    type="range"
                    min="0"
                    max="200"
                    class="w-full"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Cars Grid -->
        <div class="lg:col-span-3">
          <!-- Sort Options -->
          <div class="flex items-center justify-between mb-8">
            <p class="text-gray-600">
              Showing <span class="font-bold">{{ filteredAndSortedCars.length }}</span> results
            </p>
            <div class="flex items-center gap-2">
              <label class="text-sm text-gray-600">Sort by:</label>
              <select
                v-model="sortBy"
                class="px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-accent text-sm"
              >
                <option value="price">Price: Low to High</option>
                <option value="price-desc">Price: High to Low</option>
                <option value="name">Name: A to Z</option>
              </select>
            </div>
          </div>

          <!-- Cars Grid -->
          <div v-if="filteredAndSortedCars.length > 0" class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <CarCard v-for="car in filteredAndSortedCars" :key="car.id" :car="car" />
          </div>

          <!-- No Results -->
          <div v-else class="text-center py-16">
            <svg class="w-16 h-16 mx-auto text-gray-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            <h3 class="text-xl font-semibold text-gray-900 mb-2">No cars found</h3>
            <p class="text-gray-600">Try adjusting your filters</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
