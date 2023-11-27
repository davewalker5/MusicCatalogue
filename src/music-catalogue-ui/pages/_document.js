import { Html, Head, Main, NextScript } from "next/document";

export default function Document() {
  return (
    <Html lang="en">
      <Head>
        <link href="/css/bootstrap.min.css" rel="stylesheet" />
        <link rel="stylesheet" href="css/globals.css" />
      </Head>
      <body>
        <div className="container">
          <Main />
        </div>
        <NextScript />
      </body>
    </Html>
  );
}
