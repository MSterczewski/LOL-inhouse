import React from "react";
import "../../styles/sharedStyles.css";
export default function Button({
  onClick,
  children,
  disabled = false,
}: {
  onClick: () => void;
  children?: React.ReactNode;
  disabled?: boolean;
}) {
  return (
    <button onClick={onClick} disabled={disabled} className="button">
      {children}
    </button>
  );
}
