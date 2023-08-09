
<script setup>

const props = defineProps({
    mainWeatherTag: String,
    windSpeed: Number,
    windGustSpeed: Number,
    clouds: Number,
    summary: String
})

</script>


<template>

<div class="ml-6 flex flex-col justify-center text-lg  space-y-2  " > 
    <span class="text-xl ">{{props.mainWeatherTag}}</span>
    <div>
        <font-awesome-icon class="w-4 h-4" icon="fa-wind" />
        <span class="ml-2 font-semibold ">{{props.windSpeed}} m/s</span>
    </div>
    <span v-if="props.windGustSpeed != undefined" class="font-semibold ">{{props.windGustSpeed}} m/s, gust</span>
    <div>
        <font-awesome-icon class="w-4 h-4" icon="fa-cloud" />
        <span class="font-semibold ml-2">{{props.clouds}} (1-100) cloudy </span>
    </div>
    <div>
        <font-awesome-icon class="w-4 h-4" icon="fa-regular fa-comment" />
        <span class="ml-2 font-semibold ">{{props.summary}} </span>
    </div>
</div>

</template>

