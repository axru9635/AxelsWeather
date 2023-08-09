
<script setup>

import { ref, reactive } from 'vue'
import { RouterView } from 'vue-router'

var activeTabName = ref('');

var tabInfo = reactive({
    'today': 'The weather right now',
    'tomorrow': 'Tomorrow\'s weather' 
});

</script>

<template>

<div class="h-screen">
    <v-card class="w-[850px] h-[450px] m-10 flex flex-col " >
        <v-tabs class="bg-blue-900 text-white " v-model="activeTabName"> 
            <v-tab value="today" to="/today" >Today</v-tab> 
            <v-tab value="tomorrow" to="/tomorrow">Tomorrow</v-tab>
        </v-tabs>
        <RouterView :day="activeTabName" :title="tabInfo[activeTabName]" />
    </v-card>
</div>

</template>
