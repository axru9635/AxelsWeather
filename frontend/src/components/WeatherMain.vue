
<script setup>

const props = defineProps({
    icon: String,
    temperature: Number
})

</script>


<template>

<div class="  flex flex-col content-center  flex-wrap "> <!-- How to get these closer together? -->
    <span v-if="!props.icon" class=" w-48 h-48" />
    <img v-else class=" w-48 h-48" :src="'/src/assets/weather-icons/' + props.icon + '.svg'" />
</div>
<div class=" w-48 h-48 flex flex-col justify-center  " > 
    <span class="text-6xl ">{{props.temperature}}°C</span>
</div>

</template>
