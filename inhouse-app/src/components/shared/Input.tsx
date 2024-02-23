import React from "react";
import "../../styles/sharedStyles.css";
export default function Input({
  id,
  name,
  value,
  onChange,
  placeholder,
  disabled = false,
}: {
  id: string;
  name: string;
  value: string;
  onChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
  placeholder: string;
  disabled?: boolean;
}) {
  return (
    <input
      id={id}
      name={name}
      value={value}
      onChange={onChange}
      placeholder={placeholder}
      className="input"
      disabled={disabled}
      //TODO: VALIDATE IF REGISTER
    />
  );
}
