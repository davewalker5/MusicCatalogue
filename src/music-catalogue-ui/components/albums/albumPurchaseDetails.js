import styles from "./albumPurchaseDetails.module.css";
import DatePicker from "react-datepicker";
import { useState, useCallback } from "react";
import CurrencyInput from "react-currency-input-field";
import config from "@/config.json";
import pages from "@/helpers/navigation";
import { apiSetAlbumPurchaseDetails } from "@/helpers/api/apiAlbums";
import RetailerSelector from "../retailers/retailerSelector";

/**
 * Form to set the album purchase details for an album
 * @param {*} artist
 * @param {*} album
 * @param {*} navigate
 * @param {*} logout
 */
const AlbumPurchaseDetails = ({ artist, album, navigate, logout }) => {
  // Get the initial retailer selection and purchase date
  let initialRetailer = null;
  let initialPurchaseDate = new Date();
  if (album != null) {
    initialRetailer = album.retailer;

    if (album.purchased != null) {
      initialPurchaseDate = new Date(album.purchased);
    }
  }

  // Set up state
  const [purchaseDate, setPurchaseDate] = useState(initialPurchaseDate);
  const [price, setPrice] = useState(album.price);
  const [retailer, setRetailer] = useState(initialRetailer);
  const [errorMessage, setErrorMessage] = useState("");

  /* Callback to set album purchase details */
  const setAlbumPurchaseDetails = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Construct the values to be passed to the API
      const updatedPurchaseDate =
        album.isWishListItem != true ? purchaseDate : null;
      const updatedPrice = price != undefined ? price : null;
      const updatedRetailerId = retailer != null ? retailer.id : null;

      // Apply the updates
      const updatedAlbum = await apiSetAlbumPurchaseDetails(
        album,
        updatedPurchaseDate,
        updatedPrice,
        updatedRetailerId,
        logout
      );

      // If the returned album is valid, navigate back to the albums by artist page.
      // Otherwise, show an error
      if (updatedAlbum != null) {
        navigate({
          page: pages.albums,
          artist: artist,
          album: updatedAlbum,
          isWishList: updatedAlbum.isWishListItem,
        });
      } else {
        setErrorMessage("Error updating the album purchase details");
      }
    },
    [artist, album, price, purchaseDate, retailer, logout, navigate]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">
          {album.title} - {artist.name} - Purchase Details
        </h5>
      </div>
      <div className={styles.purchaseDetailsFormContainer}>
        <form className={styles.purchaseDetailsForm}>
          <div>
            {album.isWishListItem == true ? (
              <></>
            ) : (
              <div className="form-group mt-3">
                <label className={styles.purchaseDetailsFormLabel}>
                  Purchase Date
                </label>
                <div>
                  <DatePicker
                    selected={purchaseDate}
                    onChange={(date) => setPurchaseDate(date)}
                  />
                </div>
              </div>
            )}
            <div className="form-group mt-3">
              <label className={styles.purchaseDetailsFormLabel}>Price</label>
              <div>
                <CurrencyInput
                  placeholder="Price"
                  name="price"
                  defaultValue={price}
                  decimalsLimit={2}
                  allowNegativeValue={false}
                  disableAbbreviations={true}
                  intlConfig={{
                    locale: config.region.locale,
                    currency: config.region.currency,
                  }}
                  onValueChange={(value, _) => setPrice(value)}
                />
              </div>
            </div>
            <div className="form-group mt-3">
              <label className={styles.purchaseDetailsFormLabel}>
                Retailer
              </label>
              <div>
                <RetailerSelector
                  initialRetailer={retailer}
                  retailerChangedCallback={setRetailer}
                  logout={logout}
                />
              </div>
            </div>
            <div className="d-grid gap-2 mt-3">
              <span className={styles.purchaseDetailsError}>
                {errorMessage}
              </span>
            </div>
            <div className="d-grid gap-2 mt-3"></div>
            <div className="d-grid gap-2 mt-3"></div>
            <div className={styles.purchaseDetailsButton}>
              <button
                className="btn btn-primary"
                onClick={(e) => setAlbumPurchaseDetails(e)}
              >
                Save
              </button>
            </div>
            <div className={styles.purchaseDetailsButton}>
              <button
                className="btn btn-primary"
                onClick={() =>
                  navigate({
                    page: pages.albums,
                    artist: artist,
                    album: album,
                    isWishList: album.isWishListItem,
                  })
                }
              >
                Cancel
              </button>
            </div>
          </div>
        </form>
      </div>
    </>
  );
};

export default AlbumPurchaseDetails;
