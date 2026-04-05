import { createApp } from 'vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'

import App from './App.vue'
import { i18n } from './i18n'
import router from './router'
import { pinia } from './stores/pinia'
import './styles/base.css'

const app = createApp(App)

app.use(pinia)
app.use(i18n)
app.use(router)
app.use(ElementPlus)

app.mount('#app')
