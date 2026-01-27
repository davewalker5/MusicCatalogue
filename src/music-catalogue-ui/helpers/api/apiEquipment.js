import config from "@/config.json";
import { apiReadResponseData } from "./apiUtils";
import { apiGetPostHeaders, apiGetHeaders } from "./apiHeaders";

/**
 * Create an item of equipment
 * @param {*} equipmentTypeId
 * @param {*} manufacturerId
 * @param {*} description
 * @param {*} model
 * @param {*} serialNumber
 * @param {*} isWishListItem
 * @param {*} purchased
 * @param {*} price
 * @param {*} retailerId
 * @param {*} logout
 * @returns
 */
const apiCreateEquipment = async (
  equipmentTypeId,
  manufacturerId,
  description,
  model,
  serialNumber,
  isWishListItem,
  purchased,
  price,
  retailerId,
  logout
) => {
  // Create the request body
  const body = JSON.stringify({
    equipmentTypeId: equipmentTypeId,
    manufacturerId: manufacturerId,
    description: description,
    model: model,
    serialNumber: serialNumber,
    isWishListItem: isWishListItem,
    purchased: purchased,
    price: price,
    retailerId: retailerId,
  });

  // Call the API to create the equipment
  const url = `${config.api.baseUrl}/equipment`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const equipment = await apiReadResponseData(response, logout);
  return equipment;
};

/**
 * Update an item of equipment
 * @param {*} id
 * @param {*} equipmentTypeId
 * @param {*} manufacturerId
 * @param {*} description
 * @param {*} model
 * @param {*} serialNumber
 * @param {*} isWishListItem
 * @param {*} purchased
 * @param {*} price
 * @param {*} retailerId
 * @param {*} logout
 * @returns
 */
const apiUpdateEquipment = async (
  id,
  equipmentTypeId,
  manufacturerId,
  description,
  model,
  serialNumber,
  isWishListItem,
  purchased,
  price,
  retailerId,
  logout
) => {
  // Construct the body
  const body = JSON.stringify({
    id: id,
    equipmentTypeId: equipmentTypeId,
    manufacturerId: manufacturerId,
    description: description,
    model: model,
    serialNumber: serialNumber,
    isWishListItem: isWishListItem,
    purchased: purchased,
    price: price,
    retailerId: retailerId,
  });

  // Call the API to set the wish list flag for a given album
  const url = `${config.api.baseUrl}/equipment`;
  const response = await fetch(url, {
    method: "PUT",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const equipment = await apiReadResponseData(response, logout);
  return equipment;
};

/**
 * Set the wish list flag on an item of equipment
 * @param {*} equipment
 * @param {*} wishListFlag
 * @param {*} logout
 * @returns
 */
const apiSetEquipmentWishListFlag = async (equipment, wishListFlag, logout) => {
  // Send the update request to the API and return the response
  const response = await apiUpdateEquipment(
    equipment.id,
    equipment.equipmentTypeId,
    equipment.manufacturerId,
    equipment.description,
    equipment.model,
    equipment.serialNumber,
    wishListFlag,
    equipment.purchased,
    equipment.price,
    equipment.retailerId,
    logout
  );
  return response;
};

/**
 * Set the purchase details for the specified item of equipment
 * @param {*} equipment
 * @param {*} purchaseDate
 * @param {*} price
 * @param {*} retailerId
 * @param {*} logout
 * @returns
 */
const apiSetEquipmentPurchaseDetails = async (
  equipment,
  purchaseDate,
  price,
  retailerId,
  logout
) => {
  // Send the update request to the API and return the response
  const response = await apiUpdateEquipment(
    equipment.id,
    equipment.equipmentTypeId,
    equipment.manufacturerId,
    equipment.description,
    equipment.model,
    equipment.serialNumber,
    equipment.IsWishListItem,
    purchaseDate,
    price,
    retailerId,
    logout
  );
  return response;
};

/**
 * Delete the item of equipment with the specified ID
 * @param {*} equipmentId
 * @param {*} logout
 * @returns
 */
const apiDeleteEquipment = async (equipmentId, logout) => {
  // Call the API to delete the specified item of equipment
  const url = `${config.api.baseUrl}/equipment/${equipmentId}`;
  const response = await fetch(url, {
    method: "DELETE",
    headers: apiGetHeaders(),
  });

  if (response.status == 401) {
    // Unauthorized so the token's likely expired - force a login
    logout();
  } else {
    // Return the response status code
    return response.ok;
  }
};

/**
 * Return a list of equipment
 * @param {*} isWishList
 * @param {*} logout
 * @returns
 */
const apiFetchEquipment = async (isWishList, logout) => {
  // Construct the filtering criteria as the request body and convert to JSON
  const criteria = {
    wishList: isWishList,
  };
  const body = JSON.stringify(criteria);

  // Call the API to retrieve the matching equipment list
  const url = `${config.api.baseUrl}/equipment/search`;
  const response = await fetch(url, {
    method: "POST",
    headers: apiGetPostHeaders(),
    body: body,
  });

  const equipment = await apiReadResponseData(response, logout);
  return equipment;
};

export {
  apiCreateEquipment,
  apiUpdateEquipment,
  apiDeleteEquipment,
  apiFetchEquipment,
  apiSetEquipmentWishListFlag,
  apiSetEquipmentPurchaseDetails,
};
