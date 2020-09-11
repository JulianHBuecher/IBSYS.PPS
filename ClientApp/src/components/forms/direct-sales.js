import React from "react";
import { Table } from "reactstrap";

export default function DirectSales() {
  return (
    <form>
      <Table responsive borderless>
        <thead>
          <tr>
            <th>Produkt</th>
            <th>Menge</th>
            <th>Preis</th>
            <th>Bestand</th>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td>Kinderfahrrad</td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
          </tr>
          <tr>
            <td>Damenfahrrad</td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
          </tr>
          <tr>
            <td>Herrenfahrrad</td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
            <td>
              <input type="text" />
            </td>
          </tr>
        </tbody>
      </Table>
    </form>
  );
}
