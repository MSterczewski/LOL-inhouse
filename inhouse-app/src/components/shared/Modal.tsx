import React, { useState, ReactNode } from "react";

export function useModal() {
  const [isOpen, setisOpen] = useState(false);

  const toggle = () => {
    setisOpen(!isOpen);
  };
  return {
    isOpen,
    toggle,
  };
}

export function Modal({
  children,
  isOpen,
}: {
  children?: ReactNode;
  isOpen: boolean;
}) {
  return (
    <>
      {isOpen && (
        <div className="modal-overlay">
          <div className="modal-box">{children}</div>
        </div>
      )}
    </>
  );
}
