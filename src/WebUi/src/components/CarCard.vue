<script setup lang="ts">
import type { Car } from '../types'

defineProps<{
  car: Car
}>()
</script>

<template>
  <router-link
    :to="`/cars/${car.id}`"
    class="group bg-white rounded-lg overflow-hidden shadow-md hover:shadow-xl transition-all duration-300 cursor-pointer"
  >
    <div class="relative h-48 overflow-hidden bg-gray-200">
      <img
        :src="car.images[0]"
        :alt="car.name"
        class="w-full h-full object-cover group-hover:scale-110 transition-transform duration-300"
      />
      <div class="absolute top-4 right-4 bg-accent text-white px-3 py-1 rounded-full text-xs font-semibold capitalize">
        {{ car.category }}
      </div>
    </div>

    <div class="p-4">
      <p class="text-xs text-gray-500 uppercase font-medium">{{ car.brand }}</p>
      <h3 class="text-lg font-bold text-primary mt-1 group-hover:text-accent transition-colors">
        {{ car.name }}
      </h3>

      <div class="flex items-center gap-4 mt-3 text-sm text-gray-600">
        <div class="flex items-center gap-1">
          <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
            <path d="M5.5 13a3.5 3.5 0 01-.369-6.98 4 4 0 117.753-1.3A4.5 4.5 0 1113.5 13H11V9.413l1.293 1.293a1 1 0 001.414-1.414l-3-3a1 1 0 00-1.414 0l-3 3a1 1 0 001.414 1.414L9 9.414V13H5.5z" />
          </svg>
          {{ car.specs.seats }}
        </div>
        <div class="flex items-center gap-1">
          <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
            <path d="M4 4a2 2 0 00-2 2v4a2 2 0 002 2V6h10a2 2 0 00-2-2H4zm2 6a2 2 0 012-2h8a2 2 0 012 2v4a2 2 0 01-2 2H8a2 2 0 01-2-2v-4zm6 4a2 2 0 100-4 2 2 0 000 4z" />
          </svg>
          {{ car.specs.luggage }}
        </div>
        <div class="flex items-center gap-1">
          <svg class="w-4 h-4" fill="currentColor" viewBox="0 0 20 20">
            <path d="M2 11a1 1 0 011-1h2a1 1 0 011 1v5a1 1 0 01-1 1H3a1 1 0 01-1-1v-5zM8 7a1 1 0 011-1h2a1 1 0 011 1v9a1 1 0 01-1 1H9a1 1 0 01-1-1V7zM14 4a1 1 0 011-1h2a1 1 0 011 1v12a1 1 0 01-1 1h-2a1 1 0 01-1-1V4z" />
          </svg>
          {{ car.specs.transmission === 'automatic' ? 'AT' : 'MT' }}
        </div>
      </div>

      <div class="flex items-end justify-between mt-4 pt-4 border-t border-gray-200">
        <div>
          <p class="text-xs text-gray-500">From</p>
          <p class="text-2xl font-bold text-accent">${{ car.pricePerDay }}<span class="text-sm text-gray-600">/day</span></p>
        </div>
        <button class="px-4 py-2 bg-accent text-white rounded-lg font-medium text-sm hover:bg-accent-dark transition-colors">
          View Details
        </button>
      </div>
    </div>
  </router-link>
</template>
