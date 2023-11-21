import styles from "./albumPurchaseDetails.module.css";
import DatePicker from "react-datepicker";
import { useState, useCallback } from "react";
import CurrencyInput from "react-currency-input-field";
import config from "../config.json";
import pages from "../helpers/navigation";
import { apiCreateRetailer } from "@/helpers/apiRetailers";
import { apiSetAlbumPurchaseDetails } from "@/helpers/apiAlbums";
import FormInputField from "./formInputField";

/**
 * Form to set the album purchase details for an album
 * @param {*} artist
 * @param {*} album
 * @param {*} isWishList
 * @param {*} navigate
 * @param {*} logout
 */
const AlbumPurchaseDetails = ({ artist, album, navigate, logout }) => {
  // Get the retailer name and purchase date from the album
  const initialRetailerName =
    album["retailer"] != null ? album["retailer"]["name"] : "";
  const initialPurchaseDate =
    album.purchased != null ? new Date(album.purchased) : new Date();

  // Set up state
  const [purchaseDate, setPurchaseDate] = useState(initialPurchaseDate);
  const [price, setPrice] = useState(album.price);
  const [retailerName, setRetailerName] = useState(initialRetailerName);
  const [errorMessage, setErrorMessage] = useState("");

  /* Callback to set album purchase details */
  const setAlbumPurchaseDetails = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // See if we have a retailer name. If so, create/retrieve the retailer and
      // capture the retailer ID
      var retailerId = null;
      if (retailerName != "") {
        const retailer = await apiCreateRetailer(retailerName, logout);
        if (retailer != null) {
          retailerId = retailer.id;
        } else {
          setErrorMessage(`Error creating retailer "${retailerName}"`);
          return;
        }
      }

      // Construct the remaining values to be passed to the API
      const updatedPurchaseDate =
        album.isWishListItem == true ? null : purchaseDate;
      const updatedPrice = price == undefined ? null : price;

      // Apply the updates
      const updatedAlbum = await apiSetAlbumPurchaseDetails(
        album,
        updatedPurchaseDate,
        updatedPrice,
        retailerId,
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
    [artist, album, purchaseDate, price, retailerName, logout, navigate]
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
            <FormInputField
              label="Retailer Name"
              name="retailer"
              value={retailerName}
              setValue={setRetailerName}
            />
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
