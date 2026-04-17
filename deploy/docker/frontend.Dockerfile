FROM node:20-alpine AS builder

WORKDIR /app
COPY frontend /app
RUN npm install && npm run build

FROM nginx:1.27-alpine

COPY --from=builder /app/dist /usr/share/nginx/html
COPY deploy/nginx/frontend.conf /etc/nginx/conf.d/default.conf

USER nginx

EXPOSE 80
