import { useEffect } from "react";
import { useLocation } from "react-router-dom";

const ScrollToTop = () => {
  const { pathname } = useLocation();

  useEffect(() => {
    // Tvingar fönstret att hoppa till toppen (0, 0)
    window.scrollTo(0, 0);
  }, [pathname]); // Körs varje gång URL-vägen ändras

  return null;
};

export default ScrollToTop;