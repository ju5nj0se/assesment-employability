<script setup>
import { RouterView } from 'vue-router'
import { onMounted } from 'vue';

onMounted(() => {
  console.log('%c🚀 ASSESSMENT APP STARTED', 'color: white; background: #667eea; padding: 4px 10px; border-radius: 4px; font-weight: bold;');
  console.log('[App] LocalStorage Check:', {
    hasToken: !!localStorage.getItem('token'),
    hasUser: !!localStorage.getItem('user')
  });
});
</script>

<template>
  <RouterView />
</template>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  background-color: #f7fafc;
}
</style>
