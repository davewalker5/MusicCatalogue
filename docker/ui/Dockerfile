FROM node:20-alpine
COPY musiccatalogue.ui-1.29.0.0 /opt/musiccatalogue.ui-1.29.0.0
WORKDIR /opt/musiccatalogue.ui-1.29.0.0
RUN npm install
RUN npm run build
ENTRYPOINT [ "npm", "start" ]
