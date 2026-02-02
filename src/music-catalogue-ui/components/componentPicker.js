import pages from "@/helpers/navigation";
import ArtistList from "./artists/artistList";
import AlbumList from "./albums/albumList";
import TrackList from "./tracks/trackList";
import LookupAlbum from "./search/lookupAlbum";
import ExportData from "./dataexchange/exportData";
import GenreStatusReport from "./reports/genreStatisticsReport";
import JobStatusReport from "./reports/jobStatusReport";
import AlbumPurchaseDetails from "./albums/albumPurchaseDetails";
import ArtistStatisticsReport from "./reports/artistStatisticsReport";
import MonthlySpendReport from "./reports/monthlySpendReport";
import GenreAlbumsReport from "./reports/genreAlbumsReport";
import GenreList from "./genres/genreList";
import GenreEditor from "./genres/genreEditor";
import MoodList from "./moods/moodList";
import MoodEditor from "./moods/moodEditor";
import RetailerList from "./retailers/retailerList";
import RetailerDetails from "./retailers/retailerDetails";
import RetailerEditor from "./retailers/retailerEditor";
import TrackEditor from "./tracks/trackEditor";
import AlbumEditor from "./albums/albumEditor";
import ArtistEditor from "./artists/artistEditor";
import RetailerStatisticsReport from "./reports/retailerStatisticsReport";
import EquipmentTypeList from "./equipmentTypes/equipmentTypeList";
import EquipmentTypeEditor from "./equipmentTypes/equipmentTypeEditor";
import ManufacturerList from "./manufacturers/manufacturerList";
import ManufacturerEditor from "./manufacturers/manufacturerEditor";
import EquipmentList from "./equipment/equipmentList";
import EquipmentPurchaseDetails from "./equipment/equpimentPurchaseDetails";
import EquipmentEditor from "./equipment/equipmentEditor";
import AlbumPicker from "./albums/albumPicker";
import AlbumsByPurchaseDateReport from "./reports/albumsByPurchaseDateReport";
import ArtistMoodEditor from "./artists/artistMoodEditor";
import ClosestArtistList from "./closest/closestArtistList";
import PlaylistBuilder from "./playlists/playlistBuilder";

/**
 * Component using the current context to select and render the current page
 * @param {*} context
 * @param {*} navigate
 * @param {*} logout
 * @returns
 */
const ComponentPicker = ({ context, navigate, logout }) => {
  switch (context.page) {
    case pages.artists:
      return (
        <ArtistList
          filter={context.filter}
          genre={context.genre}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.closestArtists:
      return (
        <ClosestArtistList
          artist={context.artist}
          filter={context.filter}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.artistEditor:
      return (
        <ArtistEditor
          filter={context.filter}
          artist={context.artist}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.artistMoodEditor:
      return (
        <ArtistMoodEditor
          filter={context.filter}
          artist={context.artist}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.albums:
      return (
        <AlbumList
          artist={context.artist}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.albumEditor:
      return (
        <AlbumEditor
          artist={context.artist}
          album={context.album}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.albumPurchaseDetails:
      return (
        <AlbumPurchaseDetails
          artist={context.artist}
          album={context.album}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.albumPicker:
      return <AlbumPicker navigate={navigate} logout={logout} />;
    case pages.playlistBuilder:
      return <PlaylistBuilder navigate={navigate} logout={logout} />;
    case pages.tracks:
      return (
        <TrackList
          artist={context.artist}
          album={context.album}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.trackEditor:
      return (
        <TrackEditor
          track={context.track}
          artist={context.artist}
          album={context.album}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.genreEditor:
      return (
        <GenreEditor
          genre={context.genre}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.moodEditor:
      return (
        <MoodEditor
          mood={context.mood}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.lookup:
      return <LookupAlbum navigate={navigate} logout={logout} />;
    case pages.retailers:
      return <RetailerList navigate={navigate} logout={logout} />;
    case pages.retailerDetails:
      return (
        <RetailerDetails
          retailer={context.retailer}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.retailerEditor:
      return (
        <RetailerEditor
          retailer={context.retailer}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.genres:
      return <GenreList navigate={navigate} logout={logout} />;
    case pages.moods:
      return <MoodList navigate={navigate} logout={logout} />;
    case pages.equipmentTypes:
      return <EquipmentTypeList navigate={navigate} logout={logout} />;
    case pages.equipmentTypeEditor:
      return (
        <EquipmentTypeEditor
          equipmentType={context.equipmentType}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.manufacturers:
      return <ManufacturerList navigate={navigate} logout={logout} />;
    case pages.manufacturerEditor:
      return (
        <ManufacturerEditor
          manufacturer={context.manufacturer}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.equipment:
      return (
        <EquipmentList
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.equipmentPurchaseDetails:
      return (
        <EquipmentPurchaseDetails
          equipment={context.equipment}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.equipmentEditor:
      return (
        <EquipmentEditor
          equipment={context.equipment}
          isWishList={context.isWishList}
          navigate={navigate}
          logout={logout}
        />
      );
    case pages.export:
      return <ExportData logout={logout} />;
    case pages.artistStatisticsReport:
      return <ArtistStatisticsReport logout={logout} />;
    case pages.genreStatisticsReport:
      return <GenreStatusReport logout={logout} />;
    case pages.genreAlbumsReport:
      return <GenreAlbumsReport logout={logout} />;
    case pages.retailerStatisticsReport:
      return <RetailerStatisticsReport logout={logout} />;
    case pages.jobStatusReport:
      return <JobStatusReport logout={logout} />;
    case pages.monthlySpendReport:
      return <MonthlySpendReport logout={logout} />;
    case pages.albumsByPurchaseDateReport:
      return <AlbumsByPurchaseDateReport logout={logout} />;
    default:
      return <span />;
  }
};

export default ComponentPicker;
