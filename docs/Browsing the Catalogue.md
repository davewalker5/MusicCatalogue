## Main Catalogue

- After logging in to the UI, the "Artists" page is displayed, listing the artists currently in the database
- This acts as the home page for the site and clicking on the "Music > Artists" menu item or the site logo navigates back to it

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/artist-list.png" alt="Artist List" width="600">

- When first opened, the page shows artists with names beginning with "A"
- Clicking on a letter in the alphabet at the top of the page filters the view to show artists with names beginning with that letter
- Clicking on "All" shows all artists
- As the mouse pointer is moved up and down the table, the current row is highlighted
- Clicking on the trash icon prompts for confirmation and, if confirmed, deletes the artist shown in that row provided they do not have any albums associated with them
- Clicking on the "Edit" icon opens the artist editor to edit the artist properties:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/artist-editor.png" alt="Track List" width="600">

- Clicking on the "Add" button at the bottom of the artist list will open a blank artist editor to add and save a new artist
- Clicking on a row opens the album list for the artist shown in that row:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/album-list.png" alt="Album List" width="600">

- As the mouse pointer is moved up and down the table, the current row is highlighted
- Clicking on the trash icon prompts for confirmation and, if confirmed, deletes the album shown in that row along with the associated tracks
- Clicking on the "heart" icon moves the album from the main catalogue to the wish list then refreshes the album list
- Clicking on the "money" icon opens a form allowing the purchase details to be set:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/purchase-details.png" alt="Purchase Details" width="600">

- Note that retailers must be added using the retailer list and retailer editing page (see below) before they will appear in the drop-down on the purchase details page
- Clicking on the "Edit" icon opens the album editor to edit the album properties:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/album-editor.png" alt="Track List" width="600">

- Clicking on the "Add" button at the bottom of the album list will open a blank album editor to add and save a new album for the current artist
- Clicking anywhere else on a row opens the track list for the album shown in that row:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/track-list.png" alt="Track List" width="600">

- Clicking on the artist name in any row in the track list or clicking on the "Back" button returns to the album list for that artist
- Clicking on the "Delete" icon will prompt for confirmation and, if the action is confirmed, will delete the track
- Clicking on the "Edit" icon opens the track editor to edit the track properties:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/track-editor.png" alt="Track List" width="600">

- Clicking on the "Add" button at the bottom of the track list will open a blank track editor to add and save a new track

## Browsing By Genre

- To browse by genre, click on the "Music > Genres" menu item
- A page listing the genres derived from all albums in the //main catalogue// is displayed

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/genre-list.png" alt="Genre List" width="600">

- As the mouse pointer is moved up and down the table, the current row is highlighted
- Clicking on a row opens the artist list for the genre shown in that row

## The Wish List

- To view the wish list, click on the "Music > Wish List" menu item
- A page identical in layout to the "Artists" page is displayed, but with a title indicating that it is the wish list
- The page operates in an identical manner to the "Artists" page, using the same alphabet filter
- Clicking on a row in the table navigates to the wish list for that artist:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/wish-list-album-list.png" alt="Wish List Album List" width="600">

- Clicking on a row drills into the album content, as per the "Artists" page
- Clicking on the trash icon prompts for confirmation and, if confirmed, deletes the album shown in that row along with the associated tracks
- Clicking on the vinyl record icon moves the album from the wish list to the main catalogue then refreshes the album list
- Clicking on the money icon opens the purchase details page and allows the price and a potential retailer to be set, but not the purchase date

## The Retailers List

- To view a list of retailers in the database, click on the "Music > Retailers" menu item
- A page listing the retailers in the database is displayed:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/retailer-list.png" alt="Retailers List" width="600">

- Clicking on the trash can icon in a row will prompt for confirmation and then attempt to delete the retailer on the selected row
- Retailers that are currently "in use" (associated with an album) cannot be deleted and attempting to delete them will result in an error message being displayed
- Clicking on the "Add" button opens the retailer details editing page (see below) to add a new retailer
- Clicking on the edit icon in a row navigates to the retailer details editing page for that retailer (see below)
- Clicking on a row in the table navigates to the details viewing page for that retailer:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/retailer-details.png" alt="Retailer Details" width="600">

- The map is only displayed if:
  - Latitude and longitude are set against the retailer's record in the database
  - The UI configuration file contains an API key for Google Maps
- To edit the details for the current retailer, click on the "Edit" button to go to the retailer details editing page:

<img src="https://github.com/davewalker5/MusicCatalogue/blob/main/diagrams/retailer-editor.png" alt="Retailer Details" width="600">

- Clicking on the globe icon next to the postcode entry will geocode the current address and populate the latitude and longitude with the results
