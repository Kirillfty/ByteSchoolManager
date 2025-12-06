<template>
  <div class="container">
    <Navigation></Navigation>
    <!-- Main Content -->
    <div class="flex-1 bg-gray-50 flex">
      <p>Home</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import Navigation from '@/components/Navigation.vue';
</script>

<style scoped>
.container {
  width: 100%;
  display: flex;
  justify-content: space-between;
}


</style>
