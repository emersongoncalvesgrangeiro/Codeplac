import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'
import * as babel from '@babel/core'

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')
  return {
    define: {
      'process.env.REACT_APP_URL': JSON.stringify(env.REACT_APP_URL),
    },
    plugins: [
      {
        name: 'js-jsx-transform',
        enforce: 'pre',
        async transform(code, id) {
          if (!id.endsWith('.js') || id.includes('node_modules')) return null
          const result = await babel.transformAsync(code, {
            filename: id,
            presets: [['@babel/preset-react', { runtime: 'automatic' }]],
            sourceMaps: true,
            configFile: false,
            babelrc: false,
          })
          if (!result?.code) return null
          return { code: result.code, map: result.map ?? undefined }
        },
      },
      react(),
    ],
    publicDir: 'public',
    optimizeDeps: {
      rolldownOptions: {
        moduleTypes: {
          '.js': 'jsx',
        },
      },
    },
  }
})
