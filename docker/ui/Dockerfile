FROM node:20-alpine
WORKDIR /opt/musiccatalogue.ui

# Copy manifests first for better caching
COPY musiccatalogue.ui-1.43.0.0/package*.json ./
RUN npm ci

# Then copy the rest and build
COPY musiccatalogue.ui-1.43.0.0/ ./
RUN npm run build

ENTRYPOINT ["npm", "start"]