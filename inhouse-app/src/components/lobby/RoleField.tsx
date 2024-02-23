import React from "react";
import "../../styles/waitingPlayersStyles.css";
import PriorityIcon from "../shared/PriorityIcon.tsx";
import Select from "react-select";

export default function RoleField({
  name,
  priority,
  onChange,
  disabled = false,
}: {
  name: string;
  priority: number;
  onChange: (priority: number, position: string) => void;
  disabled?: boolean;
}) {
  function capitalizeFirstLetter(s: string) {
    return s.charAt(0).toUpperCase() + s.slice(1);
  }
  const options = [
    { priority: 1, value: "1" },
    { priority: 2, value: "2" },
    { priority: 3, value: "3" },
    { priority: 4, value: "4" },
    { priority: 5, value: "5" },
  ];

  return (
    <div className="home-page-role-field-priority-wrapper">
      <div className="home-page-role-field-priority-label">
        {capitalizeFirstLetter(name)}:
      </div>
      <Select
        options={options}
        value={{ priority: priority }}
        onChange={(p) => (p == null ? null : onChange(p.priority, name))}
        formatOptionLabel={(p) => (
          <div className="country-option">
            {PriorityIcon({ priority: p.priority, size: 2 })}
          </div>
        )}
        isDisabled={disabled}
        className="home-page-role-field-priority"
      />
    </div>
  );
}
