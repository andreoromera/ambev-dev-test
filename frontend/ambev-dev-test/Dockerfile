FROM node:lts-alpine

ENV NODE_ENV=debug

WORKDIR /usr/src/app

COPY package*.json ./
RUN npm install -g @angular/cli
RUN npm install --legacy-peer-deps

COPY . ./

EXPOSE 3000

CMD ng serve --host 0.0.0.0 --port 3000
