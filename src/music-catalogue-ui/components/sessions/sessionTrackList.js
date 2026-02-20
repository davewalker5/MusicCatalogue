import SessionTrackRow from "./sessionTrackRow";

/**
 * Component to render the ordered list of tracks in a saved session
 * @param {*} session 
 * @returns 
 */
const SessionTrackList = ({ session }) => {
    return (
        <table className="table table-hover">
        <thead>
            <tr>
                <th>No.</th>
                <th>Artist</th>
                <th>Album</th>
                <th>Track</th>
                <th>Duration</th>
            </tr>
        </thead>
        <tbody>
            {session.sessionAlbums
                .flatMap(sa =>
                    sa.album.tracks.map(track => ({
                        track,
                        album: sa.album,
                        artist: sa.album.artist
                    }))
                )
                .map(({ track, album, artist }, index) => (
                    <SessionTrackRow
                        key={track.id}
                        track={track}
                        album={album}
                        artist={artist}
                        index={index + 1}
                    />
                ))}
        </tbody>
    </table>
    );
};

export default SessionTrackList;
