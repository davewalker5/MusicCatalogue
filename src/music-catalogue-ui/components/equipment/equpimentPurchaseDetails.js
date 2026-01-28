import styles from "./equipmentPurchaseDetails.module.css";
import DatePicker from "react-datepicker";
import { useState, useCallback } from "react";
import CurrencyInput from "react-currency-input-field";
import config from "@/config.json";
import pages from "@/helpers/navigation";
import RetailerSelector from "../retailers/retailerSelector";
import { apiSetEquipmentPurchaseDetails } from "@/helpers/api/apiEquipment";

/**
 * Form to set the equipment purchase details for an item of equipment
 * @param {*} equipment
 * @param {*} navigate
 * @param {*} logout
 */
const EquipmentPurchaseDetails = ({ equipment, navigate, logout }) => {
  // Get the initial retailer selection and purchase date
  let initialRetailer = null;
  let initialPurchaseDate = new Date();
  if (equipment != null) {
    initialRetailer = equipment.retailer;

    if (equipment.purchased != null) {
      initialPurchaseDate = new Date(equipment.purchased);
    }
  }

  // Set up state
  const [purchaseDate, setPurchaseDate] = useState(initialPurchaseDate);
  const [price, setPrice] = useState(equipment.price);
  const [retailer, setRetailer] = useState(initialRetailer);
  const [errorMessage, setErrorMessage] = useState("");

  /* Callback to set album purchase details */
  const setEquipmentPurchaseDetails = useCallback(
    async (e) => {
      // Prevent the default action associated with the click event
      e.preventDefault();

      // Construct the values to be passed to the API
      const updatedPurchaseDate =
        equipment.isWishListItem != true ? purchaseDate : null;
      const updatedPrice = price != undefined ? price : null;
      const updatedRetailerId = retailer != null ? retailer.id : null;

      // Apply the updates
      const updatedEquipment = await apiSetEquipmentPurchaseDetails(
        equipment,
        updatedPurchaseDate,
        updatedPrice,
        updatedRetailerId,
        logout
      );

      // If the returned album is valid, navigate back to the albums by artist page.
      // Otherwise, show an error
      if (updatedEquipment != null) {
        navigate({
          page: pages.equipment,
          isWishList: updatedEquipment.isWishListItem,
        });
      } else {
        setErrorMessage("Error updating the equipment purchase details");
      }
    },
    [equipment, price, purchaseDate, retailer, logout, navigate]
  );

  return (
    <>
      <div className="row mb-2 pageTitle">
        <h5 className="themeFontColor text-center">
          {equipment.description} - Purchase Details
        </h5>
      </div>
      <div className={styles.purchaseDetailsFormContainer}>
        <form className={styles.purchaseDetailsForm}>
          <div>
            {equipment.isWishListItem == true ? (
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
                onClick={(e) => setEquipmentPurchaseDetails(e)}
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
                    album: equipment,
                    isWishList: equipment.isWishListItem,
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

export default EquipmentPurchaseDetails;
