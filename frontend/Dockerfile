FROM node:alpine

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

EXPOSE 5173

CMD npm run dev

# # Use nginx to serve the build
# FROM nginx:alpine
# COPY --from=0 /app/build /usr/share/nginx/html
# EXPOSE 80
# CMD ["nginx", "-g", "daemon off;"]
