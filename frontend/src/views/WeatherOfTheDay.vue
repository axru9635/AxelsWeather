
<script setup lang="ts">
import axios from 'axios'
import { reactive, ref, toRefs, nextTick, watch } from 'vue'
import { useWeatherStore } from '@/stores/weather'
import WeatherMain from '@/components/WeatherMain.vue'
import WeatherDetails from '@/components/WeatherDetails.vue'

const props = defineProps({
    day: {
        type: String,
        required: true
    },
    title: {
        type: String,
        required: true
    }
})

const { day, title } = toRefs(props);

var weatherData = reactive({}) as WeatherData;
const weatherStore = useWeatherStore();

var editLocation = ref(false);
const locationField = ref();
const apiUrl = 'https://localhost:7167/api';


class WeatherData {

    public date: Date;
    public mainWeatherTag: string;
    public summary: string;
    public tempNow: bigint;
    public windSpeed: bigint;
    public windGustSpeed: bigint;
    public clouds: bigint;
    public icon: string;
    
    constructor(
        date: Date, 
        mainWeatherTag: string, 
        summary: string, 
        tempNow: bigint,
        windSpeed: bigint,
        windGustSpeed: bigint,
        clouds: bigint,
        icon: string){
        
            this.date = date;
            this.mainWeatherTag = mainWeatherTag;
            this.summary = summary;
            this.tempNow = tempNow;
            this.windSpeed = windSpeed; 
            this.windGustSpeed = windGustSpeed;
            this.clouds = clouds;
            this.icon = icon;
    }

}

async function main(){
    
    const date: string = getDate(day.value);

    const res = await axios.get(`${apiUrl}/weatherdata/stockholm/${date}`);
    
    Object.assign(weatherData,
    new WeatherData(
        res.data.date,
        res.data.mainWeatherTag,
        res.data.summary,
        res.data.tempNow,
        res.data.windSpeed,
        res.data.windGustSpeed,
        res.data.clouds,
        res.data.icon ));
    
}

function increment(){
    weatherStore.count+=1;
}


function getDate( day: string){

    const now = new Date();
    
    if(day == 'today'){

        const [todaysDate] = now.toISOString().split('T');
        
        return todaysDate;

    }else if(day == 'tomorrow'){
        
        const nowTomorrow = new Date();
        nowTomorrow.setDate(now.getDate()+1);
        const [tomorrowsDate] =  nowTomorrow.toISOString().split('T');

        return tomorrowsDate;
    }
}

watch(day, async (newForecastDay, oldForecastDay) => {
    main();
});


main();

</script>

<template>

<div class=" flow-root h-12 m-4" >
    <!-- <br>
    <button class="bg-blue-500 " @click="increment">
        increment
    </button>
    <br>
    {{ weatherStore.count }}
    <br> -->
    <h1 class=" float-left text-4xl font-medium text-blue-900 ">{{title}}</h1> 
    <div class="flex flex-row content-center flex-wrap mr-4 float-right">
        <font-awesome-icon class="w-4 h-4 mt-2 mr-1  " icon="fa-crosshairs" />
        <div v-if="!editLocation" class=" w-24 text-xl"
        @keyup.enter=""
            @click="() => {   editLocation = !editLocation; nextTick(() => { locationField.focus() } ); }" >
            Stockholm
        </div>
        <input 
        v-show="editLocation"
        ref="locationField"
        type="text" 
        class="h-10 w-60 mt-2 border-2 border-black rounded-md border-opacity-50 " 
        @blur="editLocation=false">
    </div>
</div>

<div class=" mt-10 flex flex-row " >
    <WeatherMain 
    :icon="weatherData.icon" 
    :temperature="weatherData.tempNow" />
    <div class="h-56 flex flex-row ">
    <span class="mt-4 flex flex-col w-2  justify-center border-l-2 " />
    <WeatherDetails 
    :mainWeatherTag = "weatherData.mainWeatherTag"
    :windSpeed = "weatherData.windSpeed"
    :windGustSpeed = "weatherData.windGustSpeed"
    :clouds = "weatherData.clouds"
    :summary = "weatherData.summary"
    />
    
    <!-- WeatherDetails/-->
    </div>
</div>
    <!-- /v-card -->
</template>
