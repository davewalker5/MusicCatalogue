FROM node:20-alpine
COPY musiccatalogue.ui-1.36.0.0 /opt/musiccatalogue.ui
WORKDIR /opt/musiccatalogue.ui
RUN npm install
RUN npm run build
ENTRYPOINT [ "npm", "start" ]
