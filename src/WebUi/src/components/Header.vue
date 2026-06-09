<script setup lang="ts">
import { useRoute } from 'vue-router'

const route = useRoute()

function isActive(path: string) {
  return route.path === path || route.path.startsWith(path + '/')
}
</script>

<template>
  <header class="sticky top-0 z-50 bg-white border-b border-gray-200">
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
      <div class="flex items-center justify-between h-16">
        <router-link to="/" class="flex items-center gap-2">
          <div class="w-8 h-8 bg-accent rounded-lg flex items-center justify-center">
            <svg class="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m7.5-2.75A10.5 10.5 0 0112 2.25c-5.385 0-9.75 4.365-9.75 9.75s4.365 9.75 9.75 9.75S21.75 17.385 21.75 12c0-.464-.033-.924-.1-1.375m-15.625-1.5a6.375 6.375 0 11-12.75 0 6.375 6.375 0 0112.75 0z" />
            </svg>
          </div>
          <span class="text-xl font-bold text-primary">DriveHub</span>
        </router-link>

        <nav class="hidden md:flex items-center gap-8">
          <router-link
            to="/"
            :class="[
              'text-sm font-medium transition-colors',
              isActive('/') && !isActive('/cars')
                ? 'text-accent'
                : 'text-gray-600 hover:text-gray-900',
            ]"
          >
            Home
          </router-link>
          <router-link
            to="/cars"
            :class="[
              'text-sm font-medium transition-colors',
              isActive('/cars') ? 'text-accent' : 'text-gray-600 hover:text-gray-900',
            ]"
          >
            Browse Cars
          </router-link>
        </nav>

        <button class="md:hidden p-2 text-gray-600">
          <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 12h16M4 18h16" />
          </svg>
        </button>
      </div>
    </div>
  </header>
</template>
