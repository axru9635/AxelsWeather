<script setup lang="ts">
import axios from 'axios'
import { reactive, ref, nextTick } from 'vue'

var weatherData = reactive({}) as WeatherData;
//var loaded = false;

var editLocation = ref(false);
const locationField = ref();
//const apiUrl = 'https://localhost:32760/api';
const apiUrl = 'https://localhost:7167/api';

const iconsPath = "src/assets/weather-icons/"

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
        
    var res = await axios.get(`${apiUrl}/weatherdata`); 
    
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

main();

//border-grey rounded-md border-2

</script>

<template>

<div class=" flow-root h-12 m-4" >
    <h1 class=" float-left text-4xl font-medium text-blue-900 ">The weather for tomorrow</h1> 
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
    
    <div class="  flex flex-col content-center  flex-wrap "> <!-- How to get these closer together? -->
        
        <img class=" w-48 h-48"   :src="iconsPath + weatherData.icon + '.svg'" />    
        
    </div>
    <div class=" w-48 h-48 flex flex-col justify-center  " > 
        <span class="text-6xl ">{{weatherData.tempNow}}°C</span>
    </div>
    <div class="h-56 flex flex-row ">
    <span class="mt-4 flex flex-col w-2  justify-center border-l-2 " />
    <div class="ml-6 flex flex-col justify-center text-lg  space-y-2  " > 
        <span class="text-xl ">{{weatherData.mainWeatherTag}}</span>
        <div>
            <font-awesome-icon class="w-4 h-4" icon="fa-wind" />
            <span class="ml-2 font-semibold ">{{weatherData.windSpeed}} m/s</span>
        </div>
        <span v-if="weatherData.windGustSpeed != undefined" class="font-semibold ">{{weatherData.windGustSpeed}} m/s, gust</span>
        <div>
            <font-awesome-icon class="w-4 h-4" icon="fa-cloud" />
            <span class="font-semibold ml-2">{{weatherData.clouds}} (1-100) cloudy </span>
        </div>
        <div>
            <font-awesome-icon class="w-4 h-4" icon="fa-regular fa-comment" />
            <span class="ml-2 font-semibold ">{{weatherData.summary}} </span>
        </div>
    </div>
    </div>
    
</div>
    
</template>
