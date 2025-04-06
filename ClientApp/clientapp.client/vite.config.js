import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [react()],
    server: {
        port: 54211,
        proxy: {          
            '/api': {
                // backend URL
                target: 'https://localhost:7217',
                changeOrigin: true,
                secure: false, 
            },
        },
    },
})
