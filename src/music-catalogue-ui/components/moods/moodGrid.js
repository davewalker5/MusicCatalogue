import FormCheckBox from "../common/formCheckBox";
import useMoods from "@/hooks/useMoods";

const MoodGrid = ({ selectedMoodIds, toggleMood, logout }) => {
    // Set up state
    const { moods } = useMoods(logout);

    return (
        <div
        style={{
            display: "grid",
            gridTemplateColumns: "repeat(5, minmax(0, 1fr))",
            gap: 12,
        }}
        >
            {moods.map((m) => (
                <div key={m.id}>
                <FormCheckBox
                    label={m.name}
                    name={m.id}
                    value={selectedMoodIds.includes(Number(m.id))}
                    setValue={toggleMood}
                />
                </div>
            ))}
        </div>
    );
};

export default MoodGrid;