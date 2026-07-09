FROM node:24.18.0-alpine
ARG APPLICATION_VERSION
WORKDIR /opt/musiccatalogue.ui

# Copy manifests first for better caching
COPY musiccatalogue.ui-${APPLICATION_VERSION}/package*.json ./
RUN npm ci

# Then copy the rest and build
COPY musiccatalogue.ui-${APPLICATION_VERSION}/ ./
RUN npm run build

ENTRYPOINT ["npm", "start"]
