import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import { faCrosshairs, faWind, faCloud } from '@fortawesome/free-solid-svg-icons'
import { faComment } from '@fortawesome/free-regular-svg-icons'
import App from './App.vue'
import router from './router'
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
  components,
  directives,
})

library.add(faCrosshairs)
library.add(faWind)
library.add(faCloud)
library.add(faComment)

const app = createApp(App)

app.use(createPinia())
app.use(router)
app.use(vuetify)

app
.component('font-awesome-icon', FontAwesomeIcon)
.mount('#app')
