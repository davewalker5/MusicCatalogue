import styles from "./albumPurchaseDetails.module.css";
import DatePicker from "react-datepicker";
import { useState } from "react";
import CurrencyInput from "react-currency-input-field";
import config from "../config.json";

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
  const retailerObject = album["retailer"];
  const retailerName = retailerObject != null ? retailerObject["name"] : null;
  const currentPurchaseDate =
    album.purchased != null ? new Date(album.purchased) : new Date();

  // Set up state
  const [purchaseDate, setPurchaseDate] = useState(currentPurchaseDate);
  const [price, setPrice] = useState(album.price);
  const [retailer, setRetailer] = useState(retailerName);

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">
          {album.title} - {artist.name} - Purchase Details
        </h5>
      </div>
      <div className={styles.purchaseDetailsFormContainer}>
        <div className={styles.purchaseDetailsForm}>
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
              <input
                className="form-control mt-1"
                placeholder="Retailer Name"
                name="retailer"
                value={retailer}
                onChange={(e) => setRetailer(e.target.value)}
              />
            </div>
            <div className="d-grid gap-2 mt-3"></div>
            <div className={styles.purchaseDetailsButton}>
              <button className="btn btn-primary">Save</button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default AlbumPurchaseDetails;
